using Serilog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace KubeSharper.EventQueue
{
    public class EventQueue<T> : IEventQueue<T> 
    {
        private readonly BufferBlock<T> _items = new BufferBlock<T>();
        private readonly Dictionary<T, byte> _set = new Dictionary<T, byte>();
        private static readonly object _lock = new object();
        private readonly SemaphoreSlim _sem = new SemaphoreSlim(1);


        public int Count => _items.Count;

        public async Task<bool> TryAdd(T item)
        {
            lock (_lock)
            {
                if (!SetAdd(item)) return false; 
                _items.Post(item);
                Log.Debug($"[Queue] Enqueued {item}");
                Log.Debug($"[Queue] [ENQ] Set: {_set.Count}, Queue: {_items.Count}");
            }
            return await Task.FromResult(true);
        }

        public async Task<T> Take(CancellationToken ct = default)
        {
            var dequeued = await _items.ReceiveAsync();
            SetRemove(dequeued);
            Log.Debug($"[Queue] [DEQ] Set: {_set.Count}, Queue: {_items.Count}");
            return dequeued;
            //T dequeued;
            //lock(_lock)
            //{
            //    Log.Debug($"[Queue] Semaphore count {_sem.CurrentCount}");
            //    dequeued = _items.Dequeue();
            //    Log.Debug($"[Queue] Dequeued {dequeued}");
            //    Log.Debug($"[Queue] Semaphore count {_sem.CurrentCount}");

                
            //    Log.Debug($"[Queue] Removing {dequeued} from set");
            //    SetRemove(dequeued);
            //}
            //return dequeued;
        }

        public async IAsyncEnumerable<T> GetStream(
            [EnumeratorCancellation] CancellationToken ct = default)
        {
            await _sem.WaitAsync();
            try
            {
                while(true)
                {
                    ct.ThrowIfCancellationRequested();
                    var dequeued = await _items.ReceiveAsync();
                    SetRemove(dequeued);
                    Log.Debug($"[Queue] [DEQ] Set: {_set.Count}, Queue: {_items.Count}");
                    yield return dequeued;
                }
            }
            finally
            {
                _sem.Release();
            }
        }

        private bool SetAdd(T item)
        {
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
            }
        }
    }
}
