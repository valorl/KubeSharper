using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KubeSharper.WorkQueue
{
    public interface IWorkQueue<T>
    {

        int Count { get; }
        Task<bool> TryAdd(T item);
        Task<(bool,T)> TryTake(CancellationToken ct = default);
        Task<bool> MarkProcessed(T item);
    }
}
