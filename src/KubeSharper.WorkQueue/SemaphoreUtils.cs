using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KubeSharper.WorkQueue
{
    internal static class SemaphoreSlimExt
    {
        internal static async Task<IDisposable> Use(this SemaphoreSlim sem, CancellationToken ct = default)
        {
            await sem.WaitAsync();
            return new SemaphoreWrapper(sem);
        }

    }

    internal class SemaphoreWrapper : IDisposable
    {
        private readonly SemaphoreSlim _sem;
        private bool _disposed;

        public SemaphoreWrapper(SemaphoreSlim sem)
        {
            _sem = sem;
        }

        public void Dispose()
        {
            if (_disposed) return;
            _sem.Release();
            _disposed = true;
        }

    }
}
