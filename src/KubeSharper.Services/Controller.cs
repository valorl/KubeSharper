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
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static KubeSharper.Reconcilliation.Handlers;

namespace KubeSharper.Services
{
    public sealed class Controller : IStartable
    {
        class WatchInfo
        {
            public ISharedEventSource Source { get; }
            public string Namespace { get; }
            public EnqueueingHandler Handler { get; }
            public IDisposable Subscription { get; set; }
            public WatchInfo(ISharedEventSource source, string @namespace, EnqueueingHandler handler)
            {
                Source = source;
                Namespace = @namespace;
                Handler = handler;
            }
        }


        private readonly List<WatchInfo> _watches = new List<WatchInfo>();
        private Task _reconcileLoop;
        private Task _resyncLoop;
        private CancellationTokenSource _cts;

        public string Id => Guid.NewGuid().ToString("N")[..6];
        internal IKubernetes Client { get; set; }
        internal IReconciler Reconciler { get; set; }
        internal IEventQueue<ReconcileRequest> Queue { get; set; }
        internal IEventSourceCache Cache { get; set; }
        internal TimeSpan ResyncPeriod { get; set; }


        public Controller(Manager manager, ControllerOptions opts)
        {
            Client = manager.Client;
            Cache = manager.Cache;
            Reconciler = opts.Reconciler;
            ResyncPeriod = opts.ResyncPeriod;

            Queue = new EventQueue<ReconcileRequest>();
            manager.Add(this);
        }

        internal Controller() {}

        public void AddWatch<T>(string @namespace, EnqueueingHandler handler)
        {
            var source = Cache.GetNamespacedFor<T>(@namespace);
            _watches.Add(new WatchInfo(source, @namespace, handler));
        }

        public Task Start(CancellationToken ct = default)
        {
            _cts = (ct == CancellationToken.None) switch
            {
                true => new CancellationTokenSource(),
                false => CancellationTokenSource.CreateLinkedTokenSource(ct)
            };

            foreach (var w in _watches)
            {
                Log.Debug($"Starting source for {w.Source.ObjectType}");
                w.Source.Subscribe(w.Handler.ToEventSourceHandler(Queue));
            }

            _reconcileLoop = ReconcileLoop(_cts.Token);
            _resyncLoop = ResyncLoop(_cts.Token);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _cts.Cancel();
            foreach (var w in _watches)
            {
                w.Source.Dispose();
            }
        }

        private async Task ReconcileLoop(CancellationToken ct)
        {
            Log.Debug("Entering reconcile loop");
            var ctx = new ReconcileContext(Client);
            while(!ct.IsCancellationRequested)
            {

                Queue.TryGet(out var req);
                if (req == null)
                {
                    await Task.Delay(1000);
                    continue;
                }

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
        private async Task ResyncLoop(CancellationToken ct)
        {
            Log.Debug($"[Resync: <MAIN LOOP>] Entered.");
            while (!ct.IsCancellationRequested)
            {
                var sw = new Stopwatch();
                await Task.Delay(ResyncPeriod, ct);
                Log.Debug($"[Resync: <MAIN LOOP>] Done waiting at 0+{sw.Elapsed}");
                var tasks = _watches.Select(w => ResyncWatch(w, ct, sw));
                await Task.WhenAll(tasks);
            }
        }

        private async Task ResyncWatch(WatchInfo watch, CancellationToken ct, Stopwatch sw)
        {
            const double jitterFactor = 0.1;
            await Task.Delay(ResyncPeriod.GetJitter(jitterFactor));
            Log.Debug($"[Resync: {watch.Source.ObjectType}] Starting resync at 0+{sw.Elapsed}");

            var objects = await watch.Source.ListMetaObjects();
            Log.Debug($"[Resync: {watch.Source.ObjectType}] List found {objects.Count} objects");

            foreach (var o in objects)
            {
                if (ct.IsCancellationRequested) break;

                dynamic d = o; //TODO: Is there a better way to do this?
                var metaObj = new KubernetesV1MetaObject
                {
                    ApiVersion = d.ApiVersion,
                    Kind = d.Kind,
                    Metadata = d.Metadata
                };
                await watch.Handler(EventType.Resync, metaObj, Queue);
            }
        }
    }
}
