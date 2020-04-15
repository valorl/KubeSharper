using Serilog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace KubeSharper.EventQueue
{
    public class EventQueue<T> : IEventQueue<T> 
    {
        private readonly Queue<T> _items = new Queue<T>();
        private readonly Dictionary<T, byte> _set = new Dictionary<T, byte>();
        private static readonly object _lock = new object();
        private readonly SemaphoreSlim _sem = new SemaphoreSlim(0);


        public int Count => _items.Count;

        public async Task<bool> TryAdd(T item)
        {
            Log.Debug($"Trying to add {item}");
            lock (_lock)
            {
                if (!SetAdd(item)) return false; 
                _items.Enqueue(item);
                _sem.Release(1);
            }
            Log.Debug($"{item} enqueued");
            return await Task.FromResult(true);
        }

        public async Task<T> Take(CancellationToken ct = default)
        {
            await _sem.WaitAsync(ct);
            T dequeued;
            lock(_lock)
            {
                dequeued = _items.Dequeue();
                Log.Debug($"Dequeued {dequeued}");
                
                Log.Debug($"Removing {dequeued} from set");
                SetRemove(dequeued);
            }
            return dequeued;
        }

        public async IAsyncEnumerable<T> GetStream(
            [EnumeratorCancellation] CancellationToken ct = default)
        {
            while(!ct.IsCancellationRequested)
            {
                yield return await Take(ct);
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
