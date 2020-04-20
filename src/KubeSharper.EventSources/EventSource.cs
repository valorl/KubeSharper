using k8s;
using KubeSharper.Utils;
using Polly;
using Polly.Retry;
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
        void Start(EventSourceHandler handler);
        Task<IList<KubernetesV1MetaObject>> ListMetaObjects();
    }

    public delegate Task EventSourceHandler(EventType et, KubernetesV1MetaObject obj);


    public sealed class EventSource<T> : IEventSource
    {
        private readonly CancellationTokenSource _cts;

        internal delegate Watcher<T> WatchMaker(EventSourceHandler handler, Action<Exception> onError, Action onClosed);
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

        public void Start(EventSourceHandler handler)
        {
            Log.Debug($"Event source {ObjectType} starting");
            InitWatcher(handler);
        }

        private void InitWatcher(EventSourceHandler handler)
        {
            void OnError(Exception ex)
            {
                Log.Error(ex, $"Watch error for {typeof(T).Name}");
                InitWatcher(handler);
            }
            void OnClosed()
            {
                Log.Warning($"Watch closed for {typeof(T).Name}");
                InitWatcher(handler);
            }
            _watcher = _watchMaker(handler.Invoke, OnError, OnClosed);
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
