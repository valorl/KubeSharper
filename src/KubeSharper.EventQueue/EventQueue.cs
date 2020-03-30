using Serilog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KubeSharper.EventQueue
{
    public class EventQueue<T> : IEventQueue<T> 
    {
        private readonly Queue<T> _items = new Queue<T>();
        private readonly Dictionary<T, byte> _set = new Dictionary<T, byte>();
        private static readonly object _lock = new object();


        public int Count => _items.Count;

        public async Task<bool> TryAdd(T item)
        {
            Log.Debug($"Trying to add {item}");
            lock (_lock)
            {
                if (SetAdd(item))
                {
                    _items.Enqueue(item);
                    Log.Debug($"{item} enqueued");
                    return true;
                }
                return false;
            }
        }

        public bool TryGet(out T item)
        {
            lock(_lock)
            {
                if(_items.Count == 0)
                {
                    item = default;
                    return false;
                }

                var dequeued = _items.Dequeue();
                Log.Debug($"Dequeued {dequeued}");
                
                Log.Debug($"Removing {dequeued} from set");
                SetRemove(dequeued);

                item = dequeued;
                return true;
            }
        }

        private bool SetAdd(T item)
        {
            if (_set.ContainsKey(item))
            {
                Log.Debug($"{item} already in set");
                return false;
            }
            _set.Add(item, 1);
            Log.Debug($"{item} added to set");
            return true;
        }
        private void SetRemove(T item)
        {
            if(_set.ContainsKey(item))
            {
                _set.Remove(item);
            }
        }
    }
}
