using System.Diagnostics;
using System.Runtime.InteropServices;

namespace LoisBench;

public static class MemoryInfo
{
    public static string GetTotalMemory()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            var psi = new ProcessStartInfo();
            psi.FileName = "/bin/bash";
            psi.Arguments = "-c \"free -m | grep Mem:\"";
            psi.RedirectStandardOutput = true;
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            var process = Process.Start(psi);
            process?.WaitForExit();
            var result = process?.StandardOutput.ReadToEnd();
            var data = result?.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (data?.Length >= 4)
            {
                return data[1];
            }
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return "Memory Info not yet integrated for windows";
        }
        else
        {
            return "Memory Info not available on this OS";
        }

        return "ERROR";
    }

    public static string GetUsedMemory()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            var psi = new ProcessStartInfo();
            psi.FileName = "/bin/bash";
            psi.Arguments = "-c \"free -m | grep Mem:\"";
            psi.RedirectStandardOutput = true;
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            var process = Process.Start(psi);
            process?.WaitForExit();
            var result = process?.StandardOutput.ReadToEnd();
            var data = result?.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (data?.Length >= 4)
            {
                return data[2];
            }
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return "Memory Info not yet integrated for windows";
        }
        else
        {
            return "Memory Info not available on this OS";
        }

        return "ERROR";
    }
}