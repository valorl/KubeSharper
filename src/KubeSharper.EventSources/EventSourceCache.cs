using k8s;
using KubeSharper.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KubeSharper.EventSources
{
    public interface IEventSourceCache : IDisposable
    {
        ISharedEventSource GetNamespacedFor<T>(string @namespace);
        Task StartAll();
    }
    public class EventSourceCache : IEventSourceCache, IDisposable
    {
        private readonly Dictionary<(Type, string), ISharedEventSource> _sources;
        private readonly IEventSources _sourceProvider;
        private readonly IKubernetes _client;

        public EventSourceCache(IEventSources sourceProvider, IKubernetes client)
        {
            _sources = new Dictionary<(Type, string), ISharedEventSource>();
            _sourceProvider = sourceProvider;
            _client = client;
        }

        public ISharedEventSource GetNamespacedFor<T>(string @namespace)
        {
            ISharedEventSource source;
            if(!_sources.TryGetValue((typeof(T), @namespace), out source))
            {
                if (typeof(T).IsSubclassOf(typeof(CustomResource)))
                {
                    var s = _sourceProvider.GetNamespacedForCustom<T>(_client, @namespace);
                    source = new SharedEventSource<CustomResource>(s);
                }
                else
                {
                    var s = _sourceProvider.GetNamespacedFor<T>(_client, @namespace);
                    source = new SharedEventSource<T>(s);
                }
                _sources.Add((typeof(T), @namespace), source);
            }
            return source;
        }

        public async Task StartAll()
        {
            foreach(var s in _sources.Values)
            {
                await s.Start();
            }
        }

        public void Dispose()
        {
            foreach(var s in _sources.Values)
            {
                s.Dispose();
            }
        }
    }
}
