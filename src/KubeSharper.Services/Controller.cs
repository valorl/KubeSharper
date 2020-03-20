using k8s;
using k8s.Models;
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KubeSharper.Services
{
    public class Controller<T> where T : CustomResource
    {
        private readonly Kubernetes _client;
        private readonly IReconciler _reconciler;

        private readonly EventQueue<ReconcileRequest> _queue;

        private readonly List<WatchInfo> _watches;

        class WatchInfo
        {
            public MethodInfo Lister { get; set; }

        }

        class NativeListParams
        {
            public bool? AllowWatchBookmarks { get; set; } = null;
            public string ContinueParameter { get; set; } = null;
            public string FieldSelector { get; set; } = null;
            public string LabelSelector { get; set; } = null;
            public int? Limit { get; set; } = null;
            public string ResourceVersion { get; set; } = null;
            public int? TimeoutSeconds { get; set; } = null;
            public bool? Watch { get; set; } = null;
            public string Pretty { get; set; } = null;
            public Dictionary<string, List<string>> customHeaders { get; set; } = null;
            public CancellationToken cancellationToken { get; set; } = default;
        }

        public Controller(Kubernetes client,
            IReconciler reconciler,
            IEventQueueFactory<ReconcileRequest> queueFactory)
        {
            _client = client;
            _reconciler = reconciler;

            _queue = queueFactory.NewEventQueue();

            //Controller<CustomResource>.RegisterWatch<V1PodList>(async c => await c.ListNamespacedPodWithHttpMessagesAsync("default"));
        }

        //public static void RegisterWatch<R>(Func<Kubernetes,Task<Microsoft.Rest.HttpOperationResponse<R>>> lister)
        //{

        //}

        public void RegisterWatch<TObject>(string @namespace)
        { 
            var objectType = typeof(TObject);
            //if(objectType.Assembly != typeof(V1Pod).Assembly)
            //{
            //    var l = _client.ListNamespacedCustomObjectWithHttpMessagesAsync()
                
            //}

            var operations = typeof(Kubernetes).GetMethods()
                .Where(mi => mi.Name.StartsWith("ListNamespaced") && mi.Name.EndsWith("WithHttpMessagesAsync"));
            var listType = Type.GetType($"{objectType.FullName}List, {objectType.Assembly}");
            var returnType = typeof(Task<>).MakeGenericType(
                typeof(HttpOperationResponse<>)) .MakeGenericType();
            var op = operations.Where(mi => mi.ReturnType == returnType).First();

            var lst = _client.ListNamespacedPodWithHttpMessagesAsync("test").Result;
            lst.Watch()
            

            var invokeParams = op.GetParameters()
                .OrderBy(p => p.Position)
                .Select(p => p.Name switch {
                    "namespaceParameter" => @namespace,
                    "watch" => true,
                    _ => Type.Missing
                }).ToArray();

            async Task Emitter()
            {
                var resultObj = op.Invoke(_client, invokeParams);
                dynamic resultTask = Convert.ChangeType(resultObj, returnType);
                var response = await resultTask;


                WatcherExt.Watch(response, (Action<WatchEventType,TObject>)((WatchEventType t, TObject o) =>
                {

                }));
                //var result = op.Invoke(_client, @namespace, null, null, null, null, null, null, null, true, null, null, default(System.Threading.CancellationToken));
            }

        }









        


    }

}
