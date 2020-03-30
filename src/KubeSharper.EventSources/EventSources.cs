using k8s;
using KubeSharper.EventQueue;
using KubeSharper.Reconcilliation;
using KubeSharper.Utils;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KubeSharper.EventSources
{
    public partial class EventSources
    {
        public EventSource<CustomResource> GetNamespacedForCustom<T>(IKubernetes operations, string @namespace)
        {
            var crd = CustomResourceDefinition.For<T>();
            async Task<Watcher<CustomResource>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = operations.ListNamespacedCustomObjectWithHttpMessagesAsync(
                    crd.Group, crd.Version, @namespace, crd.Plural, watch: true);
                var watch = list.Watch(async (WatchEventType et, CustomResource obj) =>
                {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue); 
                },
                (ex) => Log.Error(ex, "Error while processing watch event"),
                () => Log.Debug($"Watch connection for {typeof(T).Name} closed"));

                return watch;
            }

            var source = new EventSource<CustomResource>(WatchMaker, typeof(T).Name);
            return source;
        }

        private void OnError<T>(Exception ex) =>
            Log.Error(ex, $"Error processing watch event for {typeof(T).Name}");
        private void OnClose<T>() => Log.Debug($"Watch connection closed for {typeof(T).Name}");

    }
}
