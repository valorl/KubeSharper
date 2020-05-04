using Serilog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace KubeSharper.WorkQueue
{
    public class WorkQueue<T> : IWorkQueue<T>
    {
        class ValueSet<T>
        {
            private const byte FLAG = 1;
            private Dictionary<T, byte> _dict = new Dictionary<T, byte>();
            public int Count => _dict.Count;
            public bool Has(T key) => _dict.ContainsKey(key);
            public bool Add(T key) => _dict.TryAdd(key, FLAG);
            public bool Delete(T key) => _dict.Remove(key);
        }

        private readonly BufferBlock<T> _queue = new BufferBlock<T>();
        private readonly ValueSet<T> _items = new ValueSet<T>();
        private readonly ValueSet<T> _processing = new ValueSet<T>();
        private readonly SemaphoreSlim _mutex = new SemaphoreSlim(1, 1);


        public int Count {
            get
            {
                _mutex.Wait();
                try { return _queue.Count; }
                finally { _mutex.Release(); }
            }
        }

        public async Task<bool> TryAdd(T item)
        {
            await _mutex.WaitAsync();
            try
            {
                if (_items.Has(item)) return false;
                _items.Add(item);
                if(!_processing.Has(item))
                {
                    _queue.Post(item);
                    Log.Debug($"[Queue] Enqueued {item}");
                    LogStats();
                }
                return true;
            }
            finally
            {
                _mutex.Release();
            }
        }

        public async Task<(bool,T)> TryTake(CancellationToken ct = default)
        {
            await _mutex.WaitAsync();
            var (success, item) = (false, default(T));
            try
            {
                item = await _queue.ReceiveAsync(ct);
                _processing.Add(item);
                _items.Delete(item);
                success = true;
            }
            catch (InvalidOperationException)
            {
                // Thrown by ReceiveAsync if queue empty
                // success already false, nothing to do here
            }
            finally
            {
                _mutex.Release();
            }
            return (success, item);
        }

        public async Task<bool> MarkProcessed(T item)
        {
            await _mutex.WaitAsync();
            try
            {
                var success = _processing.Delete(item);

                // Requeue if the item was re-added while processing
                if(_items.Has(item))
                {
                    _queue.Post(item);
                    Log.Debug($"[Queue] Items: {_items.Count}, Queue: {_queue.Count}, Proc: {_processing.Count}");
                }

                return success;
            }
            finally
            {
                _mutex.Release();
            }
        }

        private void LogStats()
        {
            Log.Debug($"[Queue] Items: {_items.Count}, Queue: {_queue.Count}, Proc: {_processing.Count}");
        }
    }

}
