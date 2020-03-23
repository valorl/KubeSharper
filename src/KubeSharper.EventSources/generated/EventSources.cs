using k8s;
using k8s.Models;
using System;
using System.Threading.Tasks;

namespace KubeSharper.EventSources
{
    public class EventSources /*: IEventSources*/
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
            async Task<Watcher<V1Pod>> WatchMaker(Action<WatchEventType, V1Pod> onEvent)
            {
                var list = await operations.ListNamespacedPodWithHttpMessagesAsync(@namespace);
                var watch = list.Watch((WatchEventType et, V1Pod pod) => { onEvent(et, pod); });
                return watch;
            }

            var source = new EventSource<V1Pod>(WatchMaker);
            return source;
        }
        private EventSource<V1Secret> V1Secret(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<V1Secret>> WatchMaker(Action<WatchEventType, V1Secret> onEvent)
            {
                var list = await operations.ListNamespacedSecretWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch((WatchEventType et, V1Secret pod) => { onEvent(et, pod); });
                return watch;
            }

            var source = new EventSource<V1Secret>(WatchMaker);
            return source;
        }
    }
}
