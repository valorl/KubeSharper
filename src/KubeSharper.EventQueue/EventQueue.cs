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
            lock (_lock)
            {
                Log.Debug($"[Queue] Set {{{string.Join(", ", _set.Keys)}}}, Queue: {_items.Count}");
                Log.Debug($"[Queue] Trying to add {item}");
                if (!SetAdd(item)) return false; 
                _items.Enqueue(item);
                Log.Debug($"[Queue] Enqueued {item}");
                _sem.Release(1);
                Log.Debug($"[Queue] Semaphore count {_sem.CurrentCount}");
            }
            return await Task.FromResult(true);
        }

        public async Task<T> Take(CancellationToken ct = default)
        {
            await _sem.WaitAsync(ct);
            T dequeued;
            lock(_lock)
            {
                Log.Debug($"[Queue] Semaphore count {_sem.CurrentCount}");
                dequeued = _items.Dequeue();
                Log.Debug($"[Queue] Dequeued {dequeued}");
                Log.Debug($"[Queue] Semaphore count {_sem.CurrentCount}");

                
                Log.Debug($"[Queue] Removing {dequeued} from set");
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
            Log.Debug($"[Queue] Set {{{string.Join(", ", _set.Keys)}}}, Queue: {_items.Count}");
            if (_set.ContainsKey(item))
            {
                Log.Debug($"[Queue] Item already in set: {item}");
                return false;
            }
            _set.Add(item, 1);
            Log.Debug($"[Queue] Item added to set: {item}");
            return true;
        }
        private void SetRemove(T item)
        {
            if(_set.ContainsKey(item))
            {
                _set.Remove(item);
                Log.Debug($"[Queue] Set {{{string.Join(", ", _set.Keys)}}}, Queue: {_items.Count}");
            }
        }
    }
}
