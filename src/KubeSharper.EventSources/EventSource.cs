using k8s;
using KubeSharper.EventQueue;
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
        Task Start(EventSourceHandler handler);
    }

    public delegate Task EventSourceHandler(WatchEventType et, KubernetesV1MetaObject obj);

    public class EventSource<T> : IEventSource 
    {
        private readonly Func<Func<WatchEventType, KubernetesV1MetaObject, Task>, Task<Watcher<T>>> _watchMaker;

        private Watcher<T> _watcher;
        public EventSource(
            Func<Func<WatchEventType, KubernetesV1MetaObject, Task>, Task<Watcher<T>>> watchMaker)
        {
            _watchMaker = watchMaker;
        }

        public async Task Start(EventSourceHandler handler)
        {
            _watcher = await _watchMaker(handler.Invoke).ConfigureAwait(false);
        }
    }

}
