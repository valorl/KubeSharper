using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KubeSharper.EventQueue
{
    public interface IEventQueue<T>
    {
        Task<bool> TryAdd(T item);
        Task<T> Take(CancellationToken ct = default);
        IAsyncEnumerable<T> GetStream(CancellationToken ct = default);
    }
}
