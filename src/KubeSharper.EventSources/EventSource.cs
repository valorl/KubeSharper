using k8s;
using KubeSharper.EventQueue;
using KubeSharper.Kubernetes;
using KubeSharper.Reconcilliation;
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


        //public async Task Start(EventQueue<ReconcileRequest> queue)
        //{
        //    async Task Handler(WatchEventType et, KubernetesV1MetaObject obj)
        //    {
        //        try
        //        {
        //            dynamic d = obj;
        //            var req = new ReconcileRequest()
        //            {
        //                ApiVersion = d.ApiVersion,
        //                Kind = d.Kind,
        //                Namespace = d.Metadata.NamespaceProperty,
        //                Name = d.Metadata.Name
        //            };
        //            if (!(await queue.TryAdd(req)))
        //            {
        //                Log.Error($"Failed adding {req.ApiVersion}/{req.Namespace}/{req.Kind}/{req.Name}");
        //            }
        //            else
        //            {
        //                Log.Information($"Added {req}");
        //            }
        //        }
        //        catch(Exception ex)
        //        {
        //            Log.ForContext("Object", obj).Error(ex, "Handler failed.");
        //        }
        //    }
        //    _watcher = await _watchMaker(Handler).ConfigureAwait(false);
        //}

        public async Task Start(EventSourceHandler handler)
        {
            _watcher = await _watchMaker(handler.Invoke).ConfigureAwait(false);
        }
    }

}
