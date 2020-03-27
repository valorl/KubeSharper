using k8s;
using k8s.Models;
using KubeSharper.EventQueue;
using KubeSharper.EventSources;
using KubeSharper.Reconcilliation;
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KubeSharper.Services
{
    public class Controller
    {
        class WatchInfo
        {
            public IEventSource Source { get; }
            public string Namespace { get; }
            public EventSourceHandler Handler { get;}
            public WatchInfo(IEventSource source, string @namespace, EventSourceHandler handler)
            {
                Source = source;
                Namespace = @namespace;
                Handler = handler;
            }
        }

        private readonly IKubernetes _client;
        private readonly IReconciler _reconciler;
        private readonly IEventQueue<ReconcileRequest> _queue;
        private readonly IEventSources _eventSources;

        private readonly List<WatchInfo> _watches;

        public Controller(IKubernetes client,
            IReconciler reconciler,
            IEventQueueFactory<ReconcileRequest> queueFactory,
            IEventSources eventSources)
        {
            _client = client;
            _reconciler = reconciler;
            _eventSources = eventSources;
            _queue = queueFactory.NewEventQueue();
        }

        public void RegisterWatch<TObject>(string @namespace, EventSourceHandler handler)
        {
            var source = _eventSources.GetNamespacedFor<TObject>(_client, @namespace);
            _watches.Add(new WatchInfo(source, @namespace, handler));
        }

        public async Task Start()
        {
            foreach (var w in _watches)
            {
                await w.Source.Start(w.Handler, _queue);
            }
        }
    }

}
