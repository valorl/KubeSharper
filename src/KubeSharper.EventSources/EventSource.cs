using k8s;
using KubeSharper.Utils;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KubeSharper.EventSources
{
    public interface IEventSource : IDisposable
    {
        string ObjectType { get; }
        bool IsRunning { get; }
        Task Start(EventSourceHandler handler);
        Task<IList<KubernetesV1MetaObject>> ListMetaObjects();
    }

    public delegate Task EventSourceHandler(EventType et, KubernetesV1MetaObject obj);


    public sealed class EventSource<T> : IEventSource
    {
        private readonly CancellationTokenSource _cts;

        internal delegate Task<Watcher<T>> WatchMaker(EventSourceHandler h);
        private readonly WatchMaker _watchMaker;

        internal delegate Task<IList<T>> Lister();
        private readonly Lister _lister;

        private Watcher<T> _watcher;

        public string ObjectType { get; }
        public bool IsRunning => _watcher.Watching;


        internal EventSource(
            WatchMaker watchMaker,
            Lister lister,
            string objectType = null,
            CancellationToken ct = default)
        {
            _watchMaker = watchMaker;
            _lister = lister;
            ObjectType = objectType ?? typeof(T).Name;

            _cts = ct == CancellationToken.None
                ? new CancellationTokenSource()
                : CancellationTokenSource.CreateLinkedTokenSource(ct);
        }

        public async Task Start(EventSourceHandler handler)
        {
            Log.Debug($"Event source {ObjectType} starting");
            _watcher = await _watchMaker(handler.Invoke).ConfigureAwait(false);
        }

        public async Task<IList<KubernetesV1MetaObject>> ListMetaObjects()
        {
            var metaObjects = new List<KubernetesV1MetaObject>();
            foreach (var o in await _lister())
            {
                dynamic d = o; //TODO: Is there a better way to do this?
                var metaObj = new KubernetesV1MetaObject
                {
                    ApiVersion = d.ApiVersion,
                    Kind = d.Kind,
                    Metadata = d.Metadata
                };
                metaObjects.Add(metaObj);
            }
            return metaObjects;
        }

        public void Dispose()
        {
            _cts.Cancel();
            _watcher.Dispose();
        }
    }
}
