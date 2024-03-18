using System.Threading;
using System.Threading.Tasks;
using LoisBench.Benchmarks;
using Microsoft.Extensions.Hosting;

namespace LoisBench.Services;

public class CpuBenchService : BackgroundService
{
    public static bool StartBench = false;
    public static int SingleCoreBenchResult = 0;
    public static bool SingleCoreBenchRunning = false;
    public static int MultiCoreBenchResult = 0;
    public static bool MultiCoreBenchRunning = false;
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(200);

            if (StartBench && !SingleCoreBenchRunning && !MultiCoreBenchRunning)
            {
                StartBench = false;
                SingleCoreBenchResult = 0;
                MultiCoreBenchResult = 0;
                SingleCoreBenchRunning = true;

                var bench = new CpuBench();
                var singleCoreResult = await bench.SinglecoreTest();

                SingleCoreBenchResult = singleCoreResult;
                SingleCoreBenchRunning = false;
                
                MultiCoreBenchRunning = true;
                
                var multiCoreResult = await bench.MulticoreTest();
                
                MultiCoreBenchResult = multiCoreResult;
                MultiCoreBenchRunning = false;
            }
        }
    }
}