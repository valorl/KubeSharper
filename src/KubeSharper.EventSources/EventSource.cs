using k8s;
using KubeSharper.EventQueue;
using KubeSharper.Reconcilliation;
using KubeSharper.Utils;
using Microsoft.CSharp.RuntimeBinder;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KubeSharper.EventSources
{
    public interface IEventSource : IDisposable
    {
        string ObjectType { get; }
        Task Start(EventSourceHandler handler, IEventQueue<ReconcileRequest> queue);
    }

    public delegate Task EventSourceHandler(EventType et, KubernetesV1MetaObject obj, IEventQueue<ReconcileRequest> queue);


    public sealed class EventSource<T> : IEventSource
    {
        private readonly CancellationTokenSource _cts;

        internal delegate Task<Watcher<T>> WatchMaker(EventSourceHandler h, IEventQueue<ReconcileRequest> q);
        private readonly WatchMaker _watchMaker;

        internal delegate Task<IList<T>> Lister();
        private readonly Lister _lister;
        private readonly TimeSpan _resyncPeriod;


        private Watcher<T> _watcher;
        private Task _resyncLoop;

        public string ObjectType { get; }



        internal EventSource(
            WatchMaker watchMaker,
            Lister lister,
            TimeSpan? resyncPeriod = null,
            CancellationToken cancellationToken = default,
            string objectType = null)
        {
            _watchMaker = watchMaker;
            _lister = lister;

            _resyncPeriod = resyncPeriod ?? TimeSpan.FromHours(10);
            ObjectType = objectType ?? typeof(T).Name;

            _cts = cancellationToken == CancellationToken.None
                ? new CancellationTokenSource()
                : CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        }

        public async Task Start(EventSourceHandler handler, IEventQueue<ReconcileRequest> queue)
        {
            Log.Debug($"Event source {ObjectType} starting");
            _watcher = await _watchMaker(handler.Invoke, queue).ConfigureAwait(false);

            _resyncLoop = ResyncLoop(handler, queue, _cts.Token);
        }

        public void Dispose()
        {
            _cts.Cancel();
            _watcher.Dispose();
        }

        private async Task ResyncLoop(
            EventSourceHandler handler, IEventQueue<ReconcileRequest> queue, CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                await Task.Delay(_resyncPeriod.WithJitter(0.1), ct);
                await ListAndHandle(handler, queue, ct);
            }
        }

        private async Task ListAndHandle(
            EventSourceHandler handler, IEventQueue<ReconcileRequest> queue, CancellationToken ct)
        {
            Log.Debug($"[Resync: {ObjectType}] Timer tick");
            var objects = await _lister();
            Log.Debug($"[Resync: {ObjectType}] List found {objects.Count} objects");
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
                await handler(EventType.Generic, metaObj, queue);
            }
        }
    }
}
