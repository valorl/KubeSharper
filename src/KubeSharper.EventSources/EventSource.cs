using k8s;
using KubeSharper.EventQueue;
using KubeSharper.Reconcilliation;
using KubeSharper.Utils;
using Microsoft.CSharp.RuntimeBinder;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KubeSharper.EventSources
{
    public interface IEventSource : IDisposable
    {
        string ObjectType { get; }
        Task Start(EventSourceHandler handler, IEventQueue<ReconcileRequest> queue);
    }

    public delegate Task EventSourceHandler(WatchEventType et, KubernetesV1MetaObject obj, IEventQueue<ReconcileRequest> queue);


    public sealed class EventSource<T> : IEventSource
    {
        internal delegate Task<Watcher<T>> WatchMaker(EventSourceHandler h, IEventQueue<ReconcileRequest> q);
        private readonly WatchMaker _watchMaker;

        private Watcher<T> _watcher;

        public string ObjectType { get; }

        internal EventSource(WatchMaker watchMaker)
        {
            _watchMaker = watchMaker;
            ObjectType = typeof(T).Name;
        }
        internal EventSource(WatchMaker watchMaker, string objectType)
        {
            _watchMaker = watchMaker;
            ObjectType = objectType;
        }

        public async Task Start(EventSourceHandler handler, IEventQueue<ReconcileRequest> queue)
        {
            Log.Debug($"Event source {ObjectType} starting");
            _watcher = await _watchMaker(handler.Invoke, queue).ConfigureAwait(false);
        }

        public void Dispose()
        {
            _watcher.Dispose();
        }
    }

}
