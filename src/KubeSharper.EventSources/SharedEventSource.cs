using KubeSharper.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubeSharper.EventSources
{
    public interface ISharedEventSource : IDisposable
    {
        string ObjectType { get; }
        bool IsRunning { get; }
        IDisposable Subscribe(EventSourceHandler handler);
        void Start();
        Task<IList<KubernetesV1MetaObject>> ListMetaObjects();
    }
    public class SharedEventSource<T> : ISharedEventSource
    {
        private readonly EventSource<T> _source;
        private readonly Dictionary<Guid, EventSourceHandler> _handlers;

        public string ObjectType => _source.ObjectType;
        public bool IsRunning => _source.IsRunning;

        public SharedEventSource(EventSource<T> source)
        {
            _source = source;
            _handlers = new Dictionary<Guid, EventSourceHandler>();
        }

        public IDisposable Subscribe(EventSourceHandler handler)
        {
            var id = Guid.NewGuid();
            AddHandler(id, handler);
            return new SharedEventSourceSubscription(() => RemoveHandler(id));
        }

        public void Start()
        {
            _source.Start(PropagateEvent);
        }

        public Task<IList<KubernetesV1MetaObject>> ListMetaObjects() => _source.ListMetaObjects();

        public void Dispose()
        {
            _source.Dispose();
        }

        private async Task PropagateEvent(EventType et, KubernetesV1MetaObject obj)
        {
            var tasks = new List<Task>();
            foreach(var handler in _handlers.Values)
            {
                tasks.Add(handler(et, obj));
            }
            await Task.WhenAll(tasks);
        }

        private void AddHandler(Guid id, EventSourceHandler handler)
        {
            lock(_handlers)
            {
                _handlers.Add(id, handler);
            }
        }
        private void RemoveHandler(Guid id)
        {
            lock(_handlers)
            {
                _handlers.Remove(id);
            }
        }
    }


    public sealed class SharedEventSourceSubscription : IDisposable
    {
        private readonly Action _disposer;
        public SharedEventSourceSubscription(Action disposer)
        {
            _disposer = disposer;
        }

        public void Dispose()
        {
            _disposer();
        }

    }
}
