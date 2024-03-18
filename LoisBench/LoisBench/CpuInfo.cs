using System.Diagnostics;
using System.Globalization;
using System.Management;
using System.Runtime.InteropServices;
using Hardware.Info;

namespace LoisBench;

public static class CpuInfo
{
    public static string GetCpuModel()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = "-c \"lscpu | grep 'Model name:'\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };

            process.Start();
            while (!process.StandardOutput.EndOfStream)
            {
                var line = process.StandardOutput.ReadLine();
                var splitLine = line.Split(':');
                if (splitLine.Length > 1 && splitLine[0].Contains("Model name"))
                {
                    return splitLine[1].Trim();
                }
            }

            process.WaitForExit();
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\HARDWARE\DESCRIPTION\System\CentralProcessor\0", "ProcessorNameString", null)?.ToString() ?? string.Empty;
        }
        else
        {
            return "CPU Info not available on this OS";
        }

        return "";
    }

    public static string GetCpuUsage()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            var procStartInfo = new ProcessStartInfo("bash",
                "-c \"top -bn1 | grep 'Cpu(s)' | sed 's/.*, *\\([0-9.]*\\)%* id.*/\\1/' | awk '{print 100 - $1}'\"")
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            var proc = new Process
            {
                StartInfo = procStartInfo
            };

            proc.Start();

            var result = proc.StandardOutput.ReadToEnd();

            proc.Close();

            return result;
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            var hardwareInfo = new HardwareInfo();
            hardwareInfo.RefreshAll();

            foreach (var cpu in hardwareInfo.CpuList)
            {
                return "" + cpu.PercentProcessorTime;
            }

            return "0";

            /*
            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PerfFormattedData_PerfOS_Processor WHERE Name='_Total'");
            foreach (var obj in searcher.Get())
            {
                return "" + double.Parse(obj["PercentProcessorTime"].ToString() ?? string.Empty);
            }

            return "0";*/
            //throw new InvalidOperationException("Could not determine CPU usage.");
        }
        else
        {
            return "CPU Info not available on this OS";
        }
    }
}