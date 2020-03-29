﻿using k8s;
using k8s.Models;
using KubeSharper.EventQueue;
using KubeSharper.Reconcilliation;
using KubeSharper.Utils;
using System;
using System.Threading.Tasks;

namespace KubeSharper.EventSources
{
    public class EventSources : IEventSources
    {

        public EventSource<T> GetNamespacedFor<T>(IKubernetes operations, string @namespace)
        {
            var t = typeof(T);
            if (t == typeof(V1Pod)) return (EventSource<T>)(object)V1Pod(operations, @namespace);
            else if (t == typeof(V1Secret)) return (EventSource<T>)(object)V1Secret(operations, @namespace);
            else throw new NotImplementedException();
        }


        private EventSource<V1Pod> V1Pod(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<V1Pod>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = await operations.ListNamespacedPodWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1Pod obj) =>
                {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue); 
                });
                return watch;
            }

            var source = new EventSource<V1Pod>(WatchMaker);
            return source;
        }
        private EventSource<V1Secret> V1Secret(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<V1Secret>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = await operations.ListNamespacedSecretWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1Secret obj) => {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue);
                });
                return watch;
            }

            var source = new EventSource<V1Secret>(WatchMaker);
            return source;
        }
    }
}
