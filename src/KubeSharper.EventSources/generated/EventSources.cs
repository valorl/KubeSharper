



using k8s;
using k8s.Models;
using KubeSharper.EventQueue;
using KubeSharper.Reconcilliation;
using KubeSharper.Utils;
using System;
using System.Threading.Tasks;

namespace KubeSharper.EventSources
{
    public partial class EventSources : IEventSources
    {

        public EventSource<T> GetNamespacedFor<T>(IKubernetes operations, string @namespace)
        {
            var t = typeof(T);
            if (t == typeof(V1alpha1RoleBinding)) return (EventSource<T>)(object)V1alpha1RoleBinding(operations, @namespace);
            else if (t == typeof(V1alpha1Role)) return (EventSource<T>)(object)V1alpha1Role(operations, @namespace);
            else if (t == typeof(V1beta1RoleBinding)) return (EventSource<T>)(object)V1beta1RoleBinding(operations, @namespace);
            else if (t == typeof(V1beta1Role)) return (EventSource<T>)(object)V1beta1Role(operations, @namespace);
            else if (t == typeof(V1alpha1PodPreset)) return (EventSource<T>)(object)V1alpha1PodPreset(operations, @namespace);
            else if (t == typeof(V1NetworkPolicy)) return (EventSource<T>)(object)V1NetworkPolicy(operations, @namespace);
            else if (t == typeof(Networkingv1beta1Ingress)) return (EventSource<T>)(object)Networkingv1beta1Ingress(operations, @namespace);
            else if (t == typeof(V1beta1PodDisruptionBudget)) return (EventSource<T>)(object)V1beta1PodDisruptionBudget(operations, @namespace);
            else if (t == typeof(V1RoleBinding)) return (EventSource<T>)(object)V1RoleBinding(operations, @namespace);
            else if (t == typeof(V1Role)) return (EventSource<T>)(object)V1Role(operations, @namespace);
            else if (t == typeof(V1beta1CronJob)) return (EventSource<T>)(object)V1beta1CronJob(operations, @namespace);
            else if (t == typeof(V2alpha1CronJob)) return (EventSource<T>)(object)V2alpha1CronJob(operations, @namespace);
            else if (t == typeof(V1Lease)) return (EventSource<T>)(object)V1Lease(operations, @namespace);
            else if (t == typeof(V1beta1Lease)) return (EventSource<T>)(object)V1beta1Lease(operations, @namespace);
            else if (t == typeof(V1alpha1EndpointSlice)) return (EventSource<T>)(object)V1alpha1EndpointSlice(operations, @namespace);
            else if (t == typeof(V1beta1Event)) return (EventSource<T>)(object)V1beta1Event(operations, @namespace);
            else if (t == typeof(V1beta1DaemonSet)) return (EventSource<T>)(object)V1beta1DaemonSet(operations, @namespace);
            else if (t == typeof(Extensionsv1beta1Deployment)) return (EventSource<T>)(object)Extensionsv1beta1Deployment(operations, @namespace);
            else if (t == typeof(Extensionsv1beta1Ingress)) return (EventSource<T>)(object)Extensionsv1beta1Ingress(operations, @namespace);
            else if (t == typeof(V1beta1NetworkPolicy)) return (EventSource<T>)(object)V1beta1NetworkPolicy(operations, @namespace);
            else if (t == typeof(V1beta1ReplicaSet)) return (EventSource<T>)(object)V1beta1ReplicaSet(operations, @namespace);
            else if (t == typeof(V1beta2DaemonSet)) return (EventSource<T>)(object)V1beta2DaemonSet(operations, @namespace);
            else if (t == typeof(V1beta2Deployment)) return (EventSource<T>)(object)V1beta2Deployment(operations, @namespace);
            else if (t == typeof(V1beta2ReplicaSet)) return (EventSource<T>)(object)V1beta2ReplicaSet(operations, @namespace);
            else if (t == typeof(V1beta2StatefulSet)) return (EventSource<T>)(object)V1beta2StatefulSet(operations, @namespace);
            else if (t == typeof(V1HorizontalPodAutoscaler)) return (EventSource<T>)(object)V1HorizontalPodAutoscaler(operations, @namespace);
            else if (t == typeof(V2beta1HorizontalPodAutoscaler)) return (EventSource<T>)(object)V2beta1HorizontalPodAutoscaler(operations, @namespace);
            else if (t == typeof(V2beta2HorizontalPodAutoscaler)) return (EventSource<T>)(object)V2beta2HorizontalPodAutoscaler(operations, @namespace);
            else if (t == typeof(V1Job)) return (EventSource<T>)(object)V1Job(operations, @namespace);
            else if (t == typeof(V1ControllerRevision)) return (EventSource<T>)(object)V1ControllerRevision(operations, @namespace);
            else if (t == typeof(V1DaemonSet)) return (EventSource<T>)(object)V1DaemonSet(operations, @namespace);
            else if (t == typeof(V1Deployment)) return (EventSource<T>)(object)V1Deployment(operations, @namespace);
            else if (t == typeof(V1ReplicaSet)) return (EventSource<T>)(object)V1ReplicaSet(operations, @namespace);
            else if (t == typeof(V1StatefulSet)) return (EventSource<T>)(object)V1StatefulSet(operations, @namespace);
            else if (t == typeof(V1beta1ControllerRevision)) return (EventSource<T>)(object)V1beta1ControllerRevision(operations, @namespace);
            else if (t == typeof(Appsv1beta1Deployment)) return (EventSource<T>)(object)Appsv1beta1Deployment(operations, @namespace);
            else if (t == typeof(V1beta1StatefulSet)) return (EventSource<T>)(object)V1beta1StatefulSet(operations, @namespace);
            else if (t == typeof(V1beta2ControllerRevision)) return (EventSource<T>)(object)V1beta2ControllerRevision(operations, @namespace);
            else if (t == typeof(V1Service)) return (EventSource<T>)(object)V1Service(operations, @namespace);
            else if (t == typeof(V1ConfigMap)) return (EventSource<T>)(object)V1ConfigMap(operations, @namespace);
            else if (t == typeof(V1Endpoints)) return (EventSource<T>)(object)V1Endpoints(operations, @namespace);
            else if (t == typeof(V1Event)) return (EventSource<T>)(object)V1Event(operations, @namespace);
            else if (t == typeof(V1LimitRange)) return (EventSource<T>)(object)V1LimitRange(operations, @namespace);
            else if (t == typeof(V1PersistentVolumeClaim)) return (EventSource<T>)(object)V1PersistentVolumeClaim(operations, @namespace);
            else if (t == typeof(V1Pod)) return (EventSource<T>)(object)V1Pod(operations, @namespace);
            else if (t == typeof(V1PodTemplate)) return (EventSource<T>)(object)V1PodTemplate(operations, @namespace);
            else if (t == typeof(V1ReplicationController)) return (EventSource<T>)(object)V1ReplicationController(operations, @namespace);
            else if (t == typeof(V1ResourceQuota)) return (EventSource<T>)(object)V1ResourceQuota(operations, @namespace);
            else if (t == typeof(V1Secret)) return (EventSource<T>)(object)V1Secret(operations, @namespace);
            else if (t == typeof(V1ServiceAccount)) return (EventSource<T>)(object)V1ServiceAccount(operations, @namespace);
            else throw new NotImplementedException();
        }


        private EventSource<V1alpha1RoleBinding> V1alpha1RoleBinding(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<V1alpha1RoleBinding>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = await operations.ListNamespacedRoleBinding1WithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1alpha1RoleBinding obj) =>
                {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue); 
                }, OnError<V1alpha1RoleBinding>, OnClose<V1alpha1RoleBinding>);
                return watch;
            }

            var source = new EventSource<V1alpha1RoleBinding>(WatchMaker);
            return source;
        }

        private EventSource<V1alpha1Role> V1alpha1Role(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<V1alpha1Role>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = await operations.ListNamespacedRole1WithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1alpha1Role obj) =>
                {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue); 
                }, OnError<V1alpha1Role>, OnClose<V1alpha1Role>);
                return watch;
            }

            var source = new EventSource<V1alpha1Role>(WatchMaker);
            return source;
        }

        private EventSource<V1beta1RoleBinding> V1beta1RoleBinding(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<V1beta1RoleBinding>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = await operations.ListNamespacedRoleBinding2WithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1beta1RoleBinding obj) =>
                {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue); 
                }, OnError<V1beta1RoleBinding>, OnClose<V1beta1RoleBinding>);
                return watch;
            }

            var source = new EventSource<V1beta1RoleBinding>(WatchMaker);
            return source;
        }

        private EventSource<V1beta1Role> V1beta1Role(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<V1beta1Role>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = await operations.ListNamespacedRole2WithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1beta1Role obj) =>
                {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue); 
                }, OnError<V1beta1Role>, OnClose<V1beta1Role>);
                return watch;
            }

            var source = new EventSource<V1beta1Role>(WatchMaker);
            return source;
        }

        private EventSource<V1alpha1PodPreset> V1alpha1PodPreset(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<V1alpha1PodPreset>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = await operations.ListNamespacedPodPresetWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1alpha1PodPreset obj) =>
                {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue); 
                }, OnError<V1alpha1PodPreset>, OnClose<V1alpha1PodPreset>);
                return watch;
            }

            var source = new EventSource<V1alpha1PodPreset>(WatchMaker);
            return source;
        }

        private EventSource<V1NetworkPolicy> V1NetworkPolicy(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<V1NetworkPolicy>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = await operations.ListNamespacedNetworkPolicy1WithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1NetworkPolicy obj) =>
                {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue); 
                }, OnError<V1NetworkPolicy>, OnClose<V1NetworkPolicy>);
                return watch;
            }

            var source = new EventSource<V1NetworkPolicy>(WatchMaker);
            return source;
        }

        private EventSource<Networkingv1beta1Ingress> Networkingv1beta1Ingress(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<Networkingv1beta1Ingress>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = await operations.ListNamespacedIngress1WithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, Networkingv1beta1Ingress obj) =>
                {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue); 
                }, OnError<Networkingv1beta1Ingress>, OnClose<Networkingv1beta1Ingress>);
                return watch;
            }

            var source = new EventSource<Networkingv1beta1Ingress>(WatchMaker);
            return source;
        }

        private EventSource<V1beta1PodDisruptionBudget> V1beta1PodDisruptionBudget(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<V1beta1PodDisruptionBudget>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = await operations.ListNamespacedPodDisruptionBudgetWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1beta1PodDisruptionBudget obj) =>
                {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue); 
                }, OnError<V1beta1PodDisruptionBudget>, OnClose<V1beta1PodDisruptionBudget>);
                return watch;
            }

            var source = new EventSource<V1beta1PodDisruptionBudget>(WatchMaker);
            return source;
        }

        private EventSource<V1RoleBinding> V1RoleBinding(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<V1RoleBinding>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = await operations.ListNamespacedRoleBindingWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1RoleBinding obj) =>
                {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue); 
                }, OnError<V1RoleBinding>, OnClose<V1RoleBinding>);
                return watch;
            }

            var source = new EventSource<V1RoleBinding>(WatchMaker);
            return source;
        }

        private EventSource<V1Role> V1Role(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<V1Role>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = await operations.ListNamespacedRoleWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1Role obj) =>
                {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue); 
                }, OnError<V1Role>, OnClose<V1Role>);
                return watch;
            }

            var source = new EventSource<V1Role>(WatchMaker);
            return source;
        }

        private EventSource<V1beta1CronJob> V1beta1CronJob(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<V1beta1CronJob>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = await operations.ListNamespacedCronJobWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1beta1CronJob obj) =>
                {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue); 
                }, OnError<V1beta1CronJob>, OnClose<V1beta1CronJob>);
                return watch;
            }

            var source = new EventSource<V1beta1CronJob>(WatchMaker);
            return source;
        }

        private EventSource<V2alpha1CronJob> V2alpha1CronJob(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<V2alpha1CronJob>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = await operations.ListNamespacedCronJob1WithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V2alpha1CronJob obj) =>
                {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue); 
                }, OnError<V2alpha1CronJob>, OnClose<V2alpha1CronJob>);
                return watch;
            }

            var source = new EventSource<V2alpha1CronJob>(WatchMaker);
            return source;
        }

        private EventSource<V1Lease> V1Lease(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<V1Lease>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = await operations.ListNamespacedLeaseWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1Lease obj) =>
                {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue); 
                }, OnError<V1Lease>, OnClose<V1Lease>);
                return watch;
            }

            var source = new EventSource<V1Lease>(WatchMaker);
            return source;
        }

        private EventSource<V1beta1Lease> V1beta1Lease(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<V1beta1Lease>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = await operations.ListNamespacedLease1WithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1beta1Lease obj) =>
                {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue); 
                }, OnError<V1beta1Lease>, OnClose<V1beta1Lease>);
                return watch;
            }

            var source = new EventSource<V1beta1Lease>(WatchMaker);
            return source;
        }

        private EventSource<V1alpha1EndpointSlice> V1alpha1EndpointSlice(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<V1alpha1EndpointSlice>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = await operations.ListNamespacedEndpointSliceWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1alpha1EndpointSlice obj) =>
                {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue); 
                }, OnError<V1alpha1EndpointSlice>, OnClose<V1alpha1EndpointSlice>);
                return watch;
            }

            var source = new EventSource<V1alpha1EndpointSlice>(WatchMaker);
            return source;
        }

        private EventSource<V1beta1Event> V1beta1Event(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<V1beta1Event>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = await operations.ListNamespacedEvent1WithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1beta1Event obj) =>
                {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue); 
                }, OnError<V1beta1Event>, OnClose<V1beta1Event>);
                return watch;
            }

            var source = new EventSource<V1beta1Event>(WatchMaker);
            return source;
        }

        private EventSource<V1beta1DaemonSet> V1beta1DaemonSet(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<V1beta1DaemonSet>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = await operations.ListNamespacedDaemonSet2WithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1beta1DaemonSet obj) =>
                {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue); 
                }, OnError<V1beta1DaemonSet>, OnClose<V1beta1DaemonSet>);
                return watch;
            }

            var source = new EventSource<V1beta1DaemonSet>(WatchMaker);
            return source;
        }

        private EventSource<Extensionsv1beta1Deployment> Extensionsv1beta1Deployment(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<Extensionsv1beta1Deployment>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = await operations.ListNamespacedDeployment3WithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, Extensionsv1beta1Deployment obj) =>
                {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue); 
                }, OnError<Extensionsv1beta1Deployment>, OnClose<Extensionsv1beta1Deployment>);
                return watch;
            }

            var source = new EventSource<Extensionsv1beta1Deployment>(WatchMaker);
            return source;
        }

        private EventSource<Extensionsv1beta1Ingress> Extensionsv1beta1Ingress(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<Extensionsv1beta1Ingress>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = await operations.ListNamespacedIngressWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, Extensionsv1beta1Ingress obj) =>
                {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue); 
                }, OnError<Extensionsv1beta1Ingress>, OnClose<Extensionsv1beta1Ingress>);
                return watch;
            }

            var source = new EventSource<Extensionsv1beta1Ingress>(WatchMaker);
            return source;
        }

        private EventSource<V1beta1NetworkPolicy> V1beta1NetworkPolicy(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<V1beta1NetworkPolicy>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = await operations.ListNamespacedNetworkPolicyWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1beta1NetworkPolicy obj) =>
                {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue); 
                }, OnError<V1beta1NetworkPolicy>, OnClose<V1beta1NetworkPolicy>);
                return watch;
            }

            var source = new EventSource<V1beta1NetworkPolicy>(WatchMaker);
            return source;
        }

        private EventSource<V1beta1ReplicaSet> V1beta1ReplicaSet(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<V1beta1ReplicaSet>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = await operations.ListNamespacedReplicaSet2WithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1beta1ReplicaSet obj) =>
                {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue); 
                }, OnError<V1beta1ReplicaSet>, OnClose<V1beta1ReplicaSet>);
                return watch;
            }

            var source = new EventSource<V1beta1ReplicaSet>(WatchMaker);
            return source;
        }

        private EventSource<V1beta2DaemonSet> V1beta2DaemonSet(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<V1beta2DaemonSet>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = await operations.ListNamespacedDaemonSet1WithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1beta2DaemonSet obj) =>
                {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue); 
                }, OnError<V1beta2DaemonSet>, OnClose<V1beta2DaemonSet>);
                return watch;
            }

            var source = new EventSource<V1beta2DaemonSet>(WatchMaker);
            return source;
        }

        private EventSource<V1beta2Deployment> V1beta2Deployment(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<V1beta2Deployment>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = await operations.ListNamespacedDeployment2WithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1beta2Deployment obj) =>
                {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue); 
                }, OnError<V1beta2Deployment>, OnClose<V1beta2Deployment>);
                return watch;
            }

            var source = new EventSource<V1beta2Deployment>(WatchMaker);
            return source;
        }

        private EventSource<V1beta2ReplicaSet> V1beta2ReplicaSet(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<V1beta2ReplicaSet>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = await operations.ListNamespacedReplicaSet1WithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1beta2ReplicaSet obj) =>
                {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue); 
                }, OnError<V1beta2ReplicaSet>, OnClose<V1beta2ReplicaSet>);
                return watch;
            }

            var source = new EventSource<V1beta2ReplicaSet>(WatchMaker);
            return source;
        }

        private EventSource<V1beta2StatefulSet> V1beta2StatefulSet(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<V1beta2StatefulSet>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = await operations.ListNamespacedStatefulSet2WithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1beta2StatefulSet obj) =>
                {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue); 
                }, OnError<V1beta2StatefulSet>, OnClose<V1beta2StatefulSet>);
                return watch;
            }

            var source = new EventSource<V1beta2StatefulSet>(WatchMaker);
            return source;
        }

        private EventSource<V1HorizontalPodAutoscaler> V1HorizontalPodAutoscaler(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<V1HorizontalPodAutoscaler>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = await operations.ListNamespacedHorizontalPodAutoscalerWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1HorizontalPodAutoscaler obj) =>
                {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue); 
                }, OnError<V1HorizontalPodAutoscaler>, OnClose<V1HorizontalPodAutoscaler>);
                return watch;
            }

            var source = new EventSource<V1HorizontalPodAutoscaler>(WatchMaker);
            return source;
        }

        private EventSource<V2beta1HorizontalPodAutoscaler> V2beta1HorizontalPodAutoscaler(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<V2beta1HorizontalPodAutoscaler>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = await operations.ListNamespacedHorizontalPodAutoscaler1WithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V2beta1HorizontalPodAutoscaler obj) =>
                {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue); 
                }, OnError<V2beta1HorizontalPodAutoscaler>, OnClose<V2beta1HorizontalPodAutoscaler>);
                return watch;
            }

            var source = new EventSource<V2beta1HorizontalPodAutoscaler>(WatchMaker);
            return source;
        }

        private EventSource<V2beta2HorizontalPodAutoscaler> V2beta2HorizontalPodAutoscaler(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<V2beta2HorizontalPodAutoscaler>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = await operations.ListNamespacedHorizontalPodAutoscaler2WithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V2beta2HorizontalPodAutoscaler obj) =>
                {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue); 
                }, OnError<V2beta2HorizontalPodAutoscaler>, OnClose<V2beta2HorizontalPodAutoscaler>);
                return watch;
            }

            var source = new EventSource<V2beta2HorizontalPodAutoscaler>(WatchMaker);
            return source;
        }

        private EventSource<V1Job> V1Job(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<V1Job>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = await operations.ListNamespacedJobWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1Job obj) =>
                {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue); 
                }, OnError<V1Job>, OnClose<V1Job>);
                return watch;
            }

            var source = new EventSource<V1Job>(WatchMaker);
            return source;
        }

        private EventSource<V1ControllerRevision> V1ControllerRevision(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<V1ControllerRevision>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = await operations.ListNamespacedControllerRevisionWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1ControllerRevision obj) =>
                {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue); 
                }, OnError<V1ControllerRevision>, OnClose<V1ControllerRevision>);
                return watch;
            }

            var source = new EventSource<V1ControllerRevision>(WatchMaker);
            return source;
        }

        private EventSource<V1DaemonSet> V1DaemonSet(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<V1DaemonSet>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = await operations.ListNamespacedDaemonSetWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1DaemonSet obj) =>
                {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue); 
                }, OnError<V1DaemonSet>, OnClose<V1DaemonSet>);
                return watch;
            }

            var source = new EventSource<V1DaemonSet>(WatchMaker);
            return source;
        }

        private EventSource<V1Deployment> V1Deployment(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<V1Deployment>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = await operations.ListNamespacedDeploymentWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1Deployment obj) =>
                {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue); 
                }, OnError<V1Deployment>, OnClose<V1Deployment>);
                return watch;
            }

            var source = new EventSource<V1Deployment>(WatchMaker);
            return source;
        }

        private EventSource<V1ReplicaSet> V1ReplicaSet(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<V1ReplicaSet>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = await operations.ListNamespacedReplicaSetWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1ReplicaSet obj) =>
                {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue); 
                }, OnError<V1ReplicaSet>, OnClose<V1ReplicaSet>);
                return watch;
            }

            var source = new EventSource<V1ReplicaSet>(WatchMaker);
            return source;
        }

        private EventSource<V1StatefulSet> V1StatefulSet(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<V1StatefulSet>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = await operations.ListNamespacedStatefulSetWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1StatefulSet obj) =>
                {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue); 
                }, OnError<V1StatefulSet>, OnClose<V1StatefulSet>);
                return watch;
            }

            var source = new EventSource<V1StatefulSet>(WatchMaker);
            return source;
        }

        private EventSource<V1beta1ControllerRevision> V1beta1ControllerRevision(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<V1beta1ControllerRevision>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = await operations.ListNamespacedControllerRevision1WithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1beta1ControllerRevision obj) =>
                {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue); 
                }, OnError<V1beta1ControllerRevision>, OnClose<V1beta1ControllerRevision>);
                return watch;
            }

            var source = new EventSource<V1beta1ControllerRevision>(WatchMaker);
            return source;
        }

        private EventSource<Appsv1beta1Deployment> Appsv1beta1Deployment(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<Appsv1beta1Deployment>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = await operations.ListNamespacedDeployment1WithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, Appsv1beta1Deployment obj) =>
                {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue); 
                }, OnError<Appsv1beta1Deployment>, OnClose<Appsv1beta1Deployment>);
                return watch;
            }

            var source = new EventSource<Appsv1beta1Deployment>(WatchMaker);
            return source;
        }

        private EventSource<V1beta1StatefulSet> V1beta1StatefulSet(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<V1beta1StatefulSet>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = await operations.ListNamespacedStatefulSet1WithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1beta1StatefulSet obj) =>
                {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue); 
                }, OnError<V1beta1StatefulSet>, OnClose<V1beta1StatefulSet>);
                return watch;
            }

            var source = new EventSource<V1beta1StatefulSet>(WatchMaker);
            return source;
        }

        private EventSource<V1beta2ControllerRevision> V1beta2ControllerRevision(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<V1beta2ControllerRevision>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = await operations.ListNamespacedControllerRevision2WithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1beta2ControllerRevision obj) =>
                {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue); 
                }, OnError<V1beta2ControllerRevision>, OnClose<V1beta2ControllerRevision>);
                return watch;
            }

            var source = new EventSource<V1beta2ControllerRevision>(WatchMaker);
            return source;
        }

        private EventSource<V1Service> V1Service(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<V1Service>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = await operations.ListNamespacedServiceWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1Service obj) =>
                {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue); 
                }, OnError<V1Service>, OnClose<V1Service>);
                return watch;
            }

            var source = new EventSource<V1Service>(WatchMaker);
            return source;
        }

        private EventSource<V1ConfigMap> V1ConfigMap(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<V1ConfigMap>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = await operations.ListNamespacedConfigMapWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1ConfigMap obj) =>
                {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue); 
                }, OnError<V1ConfigMap>, OnClose<V1ConfigMap>);
                return watch;
            }

            var source = new EventSource<V1ConfigMap>(WatchMaker);
            return source;
        }

        private EventSource<V1Endpoints> V1Endpoints(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<V1Endpoints>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = await operations.ListNamespacedEndpointsWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1Endpoints obj) =>
                {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue); 
                }, OnError<V1Endpoints>, OnClose<V1Endpoints>);
                return watch;
            }

            var source = new EventSource<V1Endpoints>(WatchMaker);
            return source;
        }

        private EventSource<V1Event> V1Event(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<V1Event>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = await operations.ListNamespacedEventWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1Event obj) =>
                {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue); 
                }, OnError<V1Event>, OnClose<V1Event>);
                return watch;
            }

            var source = new EventSource<V1Event>(WatchMaker);
            return source;
        }

        private EventSource<V1LimitRange> V1LimitRange(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<V1LimitRange>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = await operations.ListNamespacedLimitRangeWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1LimitRange obj) =>
                {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue); 
                }, OnError<V1LimitRange>, OnClose<V1LimitRange>);
                return watch;
            }

            var source = new EventSource<V1LimitRange>(WatchMaker);
            return source;
        }

        private EventSource<V1PersistentVolumeClaim> V1PersistentVolumeClaim(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<V1PersistentVolumeClaim>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = await operations.ListNamespacedPersistentVolumeClaimWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1PersistentVolumeClaim obj) =>
                {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue); 
                }, OnError<V1PersistentVolumeClaim>, OnClose<V1PersistentVolumeClaim>);
                return watch;
            }

            var source = new EventSource<V1PersistentVolumeClaim>(WatchMaker);
            return source;
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
                }, OnError<V1Pod>, OnClose<V1Pod>);
                return watch;
            }

            var source = new EventSource<V1Pod>(WatchMaker);
            return source;
        }

        private EventSource<V1PodTemplate> V1PodTemplate(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<V1PodTemplate>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = await operations.ListNamespacedPodTemplateWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1PodTemplate obj) =>
                {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue); 
                }, OnError<V1PodTemplate>, OnClose<V1PodTemplate>);
                return watch;
            }

            var source = new EventSource<V1PodTemplate>(WatchMaker);
            return source;
        }

        private EventSource<V1ReplicationController> V1ReplicationController(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<V1ReplicationController>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = await operations.ListNamespacedReplicationControllerWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1ReplicationController obj) =>
                {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue); 
                }, OnError<V1ReplicationController>, OnClose<V1ReplicationController>);
                return watch;
            }

            var source = new EventSource<V1ReplicationController>(WatchMaker);
            return source;
        }

        private EventSource<V1ResourceQuota> V1ResourceQuota(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<V1ResourceQuota>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = await operations.ListNamespacedResourceQuotaWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1ResourceQuota obj) =>
                {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue); 
                }, OnError<V1ResourceQuota>, OnClose<V1ResourceQuota>);
                return watch;
            }

            var source = new EventSource<V1ResourceQuota>(WatchMaker);
            return source;
        }

        private EventSource<V1Secret> V1Secret(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<V1Secret>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = await operations.ListNamespacedSecretWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1Secret obj) =>
                {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue); 
                }, OnError<V1Secret>, OnClose<V1Secret>);
                return watch;
            }

            var source = new EventSource<V1Secret>(WatchMaker);
            return source;
        }

        private EventSource<V1ServiceAccount> V1ServiceAccount(IKubernetes operations, string @namespace)
        {
            async Task<Watcher<V1ServiceAccount>> WatchMaker(EventSourceHandler onEvent, IEventQueue<ReconcileRequest> queue)
            {
                var list = await operations.ListNamespacedServiceAccountWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1ServiceAccount obj) =>
                {
                    var metaObj = new KubernetesV1MetaObject
                    {
                        ApiVersion = obj.ApiVersion,
                        Kind = obj.Kind,
                        Metadata = obj.Metadata
                    };
                    await onEvent(et, metaObj, queue); 
                }, OnError<V1ServiceAccount>, OnClose<V1ServiceAccount>);
                return watch;
            }

            var source = new EventSource<V1ServiceAccount>(WatchMaker);
            return source;
        }
    }
}
