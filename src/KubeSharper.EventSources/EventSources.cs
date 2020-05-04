using k8s;
using KubeSharper.WorkQueue;
using KubeSharper.Reconcilliation;
using KubeSharper.Utils;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KubeSharper.EventSources
{
    public partial class EventSources
    {
        public EventSource<CustomResource> GetNamespacedForCustom<T>(
            IKubernetes operations, string @namespace,
            CancellationToken cancellationToken = default)
        {
            var crd = CustomResourceDefinition.For<T>();

            Watcher<CustomResource> WatchMaker( EventSourceHandler onEvent, Action<Exception> onError, Action onClosed)
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
                    await onEvent(et.ToInternal(), metaObj); 
                }, onError, onClosed);

                return watch;
            }

            async Task<IList<CustomResource>> Lister()
            {
                var list = await operations.ListNamespacedCustomObjectAsync(
                    crd.Group, crd.Version, @namespace, crd.Plural);
                var jObj = (JObject)list;
                var jItems = (JArray)jObj["items"];
                return jItems.ToObject<List<CustomResource>>();
            }

            var source = new EventSource<CustomResource>(
                WatchMaker, Lister, ct: cancellationToken);
            return source;
        }

        private void OnError<T>(Exception ex) =>
            Log.Error(ex, $"Error processing watch event for {typeof(T).Name}: {{Exception}}");
        private void OnClose<T>() => Log.Debug($"Watch connection closed for {typeof(T).Name}");
    }
}
