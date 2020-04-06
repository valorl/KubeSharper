using k8s;
using k8s.Models;
using KubeSharper.EventQueue;
using KubeSharper.EventSources;
using KubeSharper.Reconcilliation;
using KubeSharper.Utils;
using Microsoft.Rest;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KubeSharper.Services
{
    public sealed class Controller : IDisposable
    {
        class WatchInfo
        {
            public IEventSource Source { get; }
            public string Namespace { get; }
            public EventSourceHandler Handler { get; }
            public WatchInfo(IEventSource source, string @namespace, EventSourceHandler handler)
            {
                Source = source;
                Namespace = @namespace;
                Handler = handler;
            }
        }
        class ReconcileWrapper : IReconciler
        {
            private readonly ReconcileFunc _reconciler;
            public ReconcileWrapper(ReconcileFunc reconciler)
            {
                _reconciler = reconciler;
            }

            public async Task<ReconcileResult> Reconcile(ReconcileRequest request)
            {
                return await _reconciler(request);
            }
        }

        private readonly IKubernetes _client;
        private readonly IReconciler _reconciler;
        private readonly IEventQueue<ReconcileRequest> _queue;
        private readonly IEventSources _eventSources;

        private readonly List<WatchInfo> _watches = new List<WatchInfo>();
        private Task _reconcileLoop;
        private CancellationTokenSource _cts;

        public Controller(IKubernetes client,
            IEventQueueFactory<ReconcileRequest> queueFactory,
            IEventSources eventSources,
            IReconciler reconciler) : this(client, queueFactory, eventSources)
        {
            _reconciler = reconciler;
        }
        public Controller(IKubernetes client,
            IEventQueueFactory<ReconcileRequest> queueFactory,
            IEventSources eventSources,
            ReconcileFunc reconciler) : this(client, queueFactory, eventSources)
        {
            _reconciler = new ReconcileWrapper(reconciler);
        }

        private Controller(IKubernetes client,
            IEventQueueFactory<ReconcileRequest> queueFactory,
            IEventSources eventSources)
        {
            _client = client;
            _eventSources = eventSources;
            _queue = queueFactory.NewEventQueue();
        }


        public void AddWatch<T>(string @namespace, EventSourceHandler handler, TimeSpan? resyncPeriod = null)
        {
            IEventSource source;
            if(typeof(T).IsSubclassOf(typeof(CustomResource)))
            {
                source = _eventSources.GetNamespacedForCustom<T>(_client, @namespace, resyncPeriod);
            }
            else
            {
                source = _eventSources.GetNamespacedFor<T>(_client, @namespace, resyncPeriod);
            }
            _watches.Add(new WatchInfo(source, @namespace, handler));
        }

        public async Task Start(CancellationToken ct = default)
        {
            _cts = (ct == CancellationToken.None) switch
            {
                true => new CancellationTokenSource(),
                false => CancellationTokenSource.CreateLinkedTokenSource(ct)
            };


            foreach (var w in _watches)
            {
                Log.Debug($"Starting source for {w.Source.ObjectType}");
                await w.Source.Start(w.Handler, _queue);
            }

            _reconcileLoop = ReconcileLoop(_cts.Token);
        }

        public void Dispose()
        {
            foreach(var w in _watches)
            {
                w.Source.Dispose();
            }
            _cts.Cancel();
        }


        private async Task ReconcileLoop(CancellationToken ct)
        {
            Log.Debug("Entering reconcile loop");
            while(!ct.IsCancellationRequested)
            {

                _queue.TryGet(out var req);
                if (req == null) continue;

                var result = await _reconciler.Reconcile(req);
                if(result.Requeue)
                {
                    _ = Requeue(req, result.RequeueAfter, ct);
                }
            }
        }

        private async Task Requeue(ReconcileRequest req, TimeSpan after, CancellationToken ct)
        {
            if(after.TotalMilliseconds > 0)
            {
                await Task.Delay(after, ct);
            }

            if (!await _queue.TryAdd(req))
            {
                Log.Error($"Failed requeueing {req.ApiVersion}/{req.Namespace}/{req.Kind}/{req.Name}");
            }
            else
            {
                Log.Information($"Requeued {req}");
            }
        }
    }
}
