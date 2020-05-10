using BenchmarkDotNet.Attributes;
using KubeSharper.Reconcilliation;
using KubeSharper.WorkQueue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubeSharper.Benchmarks
{
    public class WorkQueueTryAdd
    {
        private readonly IWorkQueue<ReconcileRequest>_queue = new WorkQueue<ReconcileRequest>();
        private readonly ReconcileRequest _sample = new ReconcileRequest
        {
            ApiVersion = "sample.valorl.dev/v1",
            Kind = "Sample",
            Name = "sample-sample",
            Namespace = "samples"
        };

        [Benchmark]
        public async Task<bool> TryAdd()
        {
            return await _queue.TryAdd(_sample);
        }
    }
}
