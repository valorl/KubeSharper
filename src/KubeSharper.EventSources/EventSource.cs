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
    public interface IEventSource
    {
        Task Start(EventSourceHandler handler, IEventQueue<ReconcileRequest> queue);
    }

    public delegate Task EventSourceHandler(WatchEventType et, KubernetesV1MetaObject obj, IEventQueue<ReconcileRequest> queue);


    public class EventSource<T> : IEventSource
    {
        internal delegate Task<Watcher<T>> WatchMaker(EventSourceHandler h, IEventQueue<ReconcileRequest> q);
        private readonly WatchMaker _watchMaker;

        private Watcher<T> _watcher;

        internal EventSource(WatchMaker watchMaker)
        {
            _watchMaker = watchMaker;
        }

        public async Task Start(EventSourceHandler handler, IEventQueue<ReconcileRequest> queue)
        {
            _watcher = await _watchMaker(handler.Invoke, queue).ConfigureAwait(false);
        }
    }

}
