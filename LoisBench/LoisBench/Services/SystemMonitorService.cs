using Hardware.Info;

namespace LoisBench.Services;

public class SystemMonitorService : BackgroundService
{
    private HardwareInfo? _hardwareInfo;

    public static string OSName = "";
    public static string OSVersion = "";
    
    public static string CpuName = "";
    public static string CpuUsage = "0";
    public static string MemoryUsage = "0";
    public static string MemoryTotal = "0";
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_hardwareInfo is null)
        {
            _hardwareInfo = new HardwareInfo();
        }
        _hardwareInfo.RefreshAll();
        foreach (var cpu in _hardwareInfo.CpuList)
        {
            CpuName = cpu.Name;
        }
        MemoryTotal = "" + (_hardwareInfo.MemoryStatus.TotalPhysical / 1000000);
        OSName = _hardwareInfo.OperatingSystem.Name;
        OSVersion = _hardwareInfo.OperatingSystem.VersionString;
        
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(500);
            
            _hardwareInfo.RefreshCPUList();
            _hardwareInfo.RefreshMemoryStatus();
            
            foreach (var cpu in _hardwareInfo.CpuList)
            {
                CpuUsage = "" + cpu.PercentProcessorTime;
            }

            MemoryUsage = "" + ((_hardwareInfo.MemoryStatus.TotalPhysical -
                                _hardwareInfo.MemoryStatus.AvailablePhysical) / 1000000);
        }
    }
}