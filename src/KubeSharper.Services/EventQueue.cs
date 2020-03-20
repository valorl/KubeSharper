using System;
using System.Collections.Concurrent;
using System.Threading;

namespace KubeSharper.Services
{
    public class EventQueue<T>
    {
        private readonly ConcurrentQueue<T> _items = new ConcurrentQueue<T>();

        public bool TryAdd(T item)
        {
            _items.Enqueue(item);
            return true;
        }

        public bool TryGet(out T item)
        {
            return _items.TryDequeue(out item);
        }
    }
}
