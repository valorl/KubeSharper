using BenchmarkDotNet.Running;
using System;

namespace KubeSharper.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<WorkQueueTryAdd>();
        }
    }
}
