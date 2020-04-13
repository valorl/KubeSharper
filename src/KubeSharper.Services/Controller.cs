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
    public sealed class Controller : IStartable
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

        public string Id => Guid.NewGuid().ToString("N")[..6];

        internal IKubernetes Client { get; set; }
        internal IReconciler Reconciler { get; set; }
        internal IEventQueue<ReconcileRequest> Queue { get; set; }
        internal IEventSources EventSources { get; set; }

        private readonly List<WatchInfo> _watches = new List<WatchInfo>();
        private Task _reconcileLoop;
        private CancellationTokenSource _cts;

        public Controller(Manager manager, IReconciler reconciler) : this(manager)
        {
            Reconciler = reconciler;
        }
        public Controller(Manager manager, ReconcileFunc func) : this(manager)
        {
            Reconciler = func.ToReconciler();
        }

        internal Controller(Manager manager)
        {
            Client = manager.Client;
            EventSources = manager.EventSources;
            Queue = new EventQueue<ReconcileRequest>();

            manager.Add(this);
        }

        internal Controller() {}

        public void AddWatch<T>(string @namespace, EventSourceHandler handler, TimeSpan? resyncPeriod = null)
        {
            IEventSource source;
            if (typeof(T).IsSubclassOf(typeof(CustomResource)))
            {
                source = EventSources.GetNamespacedForCustom<T>(Client, @namespace, resyncPeriod);
            }
            else
            {
                source = EventSources.GetNamespacedFor<T>(Client, @namespace, resyncPeriod);
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
                await w.Source.Start(w.Handler, Queue);
            }

            _reconcileLoop = ReconcileLoop(_cts.Token);
        }

        public void Dispose()
        {
            _cts.Cancel();
            foreach (var w in _watches)
            {
                w.Source.Dispose();
            }
        }

        private void Initialize(Manager manager)
        {
            //TODO
            return;
        }


        private async Task ReconcileLoop(CancellationToken ct)
        {
            Log.Debug("Entering reconcile loop");
            var ctx = new ReconcileContext(Client);
            while(!ct.IsCancellationRequested)
            {

                Queue.TryGet(out var req);
                if (req == null) continue;

                var result = await Reconciler.Reconcile(ctx, req);
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

            if (!await Queue.TryAdd(req))
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
