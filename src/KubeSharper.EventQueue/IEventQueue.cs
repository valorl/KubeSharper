using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KubeSharper.EventQueue
{
    public interface IEventQueue<T>
    {
        Task<bool> TryAdd(T item);
        bool TryGet(out T item);
    }
}
