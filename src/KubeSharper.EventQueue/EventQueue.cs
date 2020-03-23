using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KubeSharper.EventQueue
{
    public class EventQueue<T>
    {
        private readonly ConcurrentQueue<T> _items = new ConcurrentQueue<T>();
        private readonly ConcurrentDictionary<T, bool> _set = new ConcurrentDictionary<T, bool>();


        public int Count => _items.Count;

        public async Task<bool> TryAdd(T item)
        {
            if(_set.TryAdd(item, true))
            {
                _items.Enqueue(item);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TryGet(out T item)
        {
            return _items.TryDequeue(out item);
        }

    }
}
