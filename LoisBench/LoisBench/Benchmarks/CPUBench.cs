using System;
using System.Linq;
using System.Threading.Tasks;

namespace LoisBench.Benchmarks;

public class CpuBench {
    public async Task<int> SinglecoreTest()
    {
        Console.WriteLine("Starting Singlecore Test...");
        return await CoreTest(1);
    }

    public async Task<int> MulticoreTest()
    {
        Console.WriteLine("Starting Multicore Test...");
        return await CoreTest(Environment.ProcessorCount);
    }
    
    private async Task<int> CoreTest(int coreCount = 1)
    {
        var finalCounter = 0;
        var runs = 0;
        for(var i = 0; i < 8; i++)
        {
            var intCounter = 0;
            Enumerable
                .Range(1, coreCount)
                .AsParallel()
                .Select(i => {
                    var end = DateTime.Now + TimeSpan.FromSeconds(20);
                    while (DateTime.Now < end)
                    {
                        FindPrimeNumber(1000);
                        intCounter++;
                    }
                    return i;
                })
                .ToList();
            finalCounter += intCounter;
            runs++;
            await Task.Delay(1);
        }
        if(runs <= 0) return 0;
    
        finalCounter /= runs;
        finalCounter /= (runs * 5);
    
        return finalCounter;
    }
    
    private long FindPrimeNumber(int n)
    {
        int count=0;
        long a = 2;
        while(count<n)
        {
            long b = 2;
            int prime = 1;
            while(b * b <= a)
            {
                if(a % b == 0)
                {
                    prime = 0;
                    break;
                }
                b++;
            }
            if(prime > 0)
            {
                count++;
            }
            a++;
        }
        return (--a);
    }
}