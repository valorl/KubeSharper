using k8s;
using k8s.Models;
using KubeSharper.Utils;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace KubeSharper.EventSources
{
    public partial class EventSources : IEventSources
    {

        public EventSource<T> GetNamespacedFor<T>(IKubernetes operations, string @namespace, CancellationToken cancellationToken = default)
        {
            var t = typeof(T);
            if (t == typeof(V1alpha1RoleBinding)) return (EventSource<T>)(object)V1alpha1RoleBinding(operations, @namespace, cancellationToken);
            else if (t == typeof(V1alpha1Role)) return (EventSource<T>)(object)V1alpha1Role(operations, @namespace, cancellationToken);
            else if (t == typeof(V1beta1RoleBinding)) return (EventSource<T>)(object)V1beta1RoleBinding(operations, @namespace, cancellationToken);
            else if (t == typeof(V1beta1Role)) return (EventSource<T>)(object)V1beta1Role(operations, @namespace, cancellationToken);
            else if (t == typeof(V1alpha1PodPreset)) return (EventSource<T>)(object)V1alpha1PodPreset(operations, @namespace, cancellationToken);
            else if (t == typeof(V1NetworkPolicy)) return (EventSource<T>)(object)V1NetworkPolicy(operations, @namespace, cancellationToken);
            else if (t == typeof(Networkingv1beta1Ingress)) return (EventSource<T>)(object)Networkingv1beta1Ingress(operations, @namespace, cancellationToken);
            else if (t == typeof(V1beta1PodDisruptionBudget)) return (EventSource<T>)(object)V1beta1PodDisruptionBudget(operations, @namespace, cancellationToken);
            else if (t == typeof(V1RoleBinding)) return (EventSource<T>)(object)V1RoleBinding(operations, @namespace, cancellationToken);
            else if (t == typeof(V1Role)) return (EventSource<T>)(object)V1Role(operations, @namespace, cancellationToken);
            else if (t == typeof(V2beta2HorizontalPodAutoscaler)) return (EventSource<T>)(object)V2beta2HorizontalPodAutoscaler(operations, @namespace, cancellationToken);
            else if (t == typeof(V1Job)) return (EventSource<T>)(object)V1Job(operations, @namespace, cancellationToken);
            else if (t == typeof(V1beta1CronJob)) return (EventSource<T>)(object)V1beta1CronJob(operations, @namespace, cancellationToken);
            else if (t == typeof(V2alpha1CronJob)) return (EventSource<T>)(object)V2alpha1CronJob(operations, @namespace, cancellationToken);
            else if (t == typeof(V1Lease)) return (EventSource<T>)(object)V1Lease(operations, @namespace, cancellationToken);
            else if (t == typeof(V1beta1Lease)) return (EventSource<T>)(object)V1beta1Lease(operations, @namespace, cancellationToken);
            else if (t == typeof(V1beta1EndpointSlice)) return (EventSource<T>)(object)V1beta1EndpointSlice(operations, @namespace, cancellationToken);
            else if (t == typeof(V1beta1Event)) return (EventSource<T>)(object)V1beta1Event(operations, @namespace, cancellationToken);
            else if (t == typeof(Extensionsv1beta1Ingress)) return (EventSource<T>)(object)Extensionsv1beta1Ingress(operations, @namespace, cancellationToken);
            else if (t == typeof(V1ControllerRevision)) return (EventSource<T>)(object)V1ControllerRevision(operations, @namespace, cancellationToken);
            else if (t == typeof(V1DaemonSet)) return (EventSource<T>)(object)V1DaemonSet(operations, @namespace, cancellationToken);
            else if (t == typeof(V1Deployment)) return (EventSource<T>)(object)V1Deployment(operations, @namespace, cancellationToken);
            else if (t == typeof(V1ReplicaSet)) return (EventSource<T>)(object)V1ReplicaSet(operations, @namespace, cancellationToken);
            else if (t == typeof(V1StatefulSet)) return (EventSource<T>)(object)V1StatefulSet(operations, @namespace, cancellationToken);
            else if (t == typeof(V1HorizontalPodAutoscaler)) return (EventSource<T>)(object)V1HorizontalPodAutoscaler(operations, @namespace, cancellationToken);
            else if (t == typeof(V2beta1HorizontalPodAutoscaler)) return (EventSource<T>)(object)V2beta1HorizontalPodAutoscaler(operations, @namespace, cancellationToken);
            else if (t == typeof(V1Service)) return (EventSource<T>)(object)V1Service(operations, @namespace, cancellationToken);
            else if (t == typeof(V1ConfigMap)) return (EventSource<T>)(object)V1ConfigMap(operations, @namespace, cancellationToken);
            else if (t == typeof(V1Endpoints)) return (EventSource<T>)(object)V1Endpoints(operations, @namespace, cancellationToken);
            else if (t == typeof(V1Event)) return (EventSource<T>)(object)V1Event(operations, @namespace, cancellationToken);
            else if (t == typeof(V1LimitRange)) return (EventSource<T>)(object)V1LimitRange(operations, @namespace, cancellationToken);
            else if (t == typeof(V1PersistentVolumeClaim)) return (EventSource<T>)(object)V1PersistentVolumeClaim(operations, @namespace, cancellationToken);
            else if (t == typeof(V1Pod)) return (EventSource<T>)(object)V1Pod(operations, @namespace, cancellationToken);
            else if (t == typeof(V1PodTemplate)) return (EventSource<T>)(object)V1PodTemplate(operations, @namespace, cancellationToken);
            else if (t == typeof(V1ReplicationController)) return (EventSource<T>)(object)V1ReplicationController(operations, @namespace, cancellationToken);
            else if (t == typeof(V1ResourceQuota)) return (EventSource<T>)(object)V1ResourceQuota(operations, @namespace, cancellationToken);
            else if (t == typeof(V1Secret)) return (EventSource<T>)(object)V1Secret(operations, @namespace, cancellationToken);
            else if (t == typeof(V1ServiceAccount)) return (EventSource<T>)(object)V1ServiceAccount(operations, @namespace, cancellationToken);
            else throw new NotImplementedException();
        }


        private EventSource<V1alpha1RoleBinding> V1alpha1RoleBinding(IKubernetes operations, string @namespace, CancellationToken ct)
        {
            Watcher<V1alpha1RoleBinding> WatchMaker(EventSourceHandler onEvent, Action<Exception> onError, Action onClosed)
            {
                var list = operations.ListNamespacedRoleBinding1WithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1alpha1RoleBinding obj) =>
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

            async Task<IList<V1alpha1RoleBinding>> Lister()
            {
                var list = await operations.ListNamespacedRoleBinding1Async(@namespace);
                var kind = list.Kind.Replace("List", "");
                foreach (var i in list.Items)
                {
                    i.ApiVersion = list.ApiVersion;
                    i.Kind = kind;
                }
                return list.Items;
            }

            var source = new EventSource<V1alpha1RoleBinding>(WatchMaker, Lister, ct: ct);
            return source;
        }

        private EventSource<V1alpha1Role> V1alpha1Role(IKubernetes operations, string @namespace, CancellationToken ct)
        {
            Watcher<V1alpha1Role> WatchMaker(EventSourceHandler onEvent, Action<Exception> onError, Action onClosed)
            {
                var list = operations.ListNamespacedRole1WithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1alpha1Role obj) =>
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

            async Task<IList<V1alpha1Role>> Lister()
            {
                var list = await operations.ListNamespacedRole1Async(@namespace);
                var kind = list.Kind.Replace("List", "");
                foreach (var i in list.Items)
                {
                    i.ApiVersion = list.ApiVersion;
                    i.Kind = kind;
                }
                return list.Items;
            }

            var source = new EventSource<V1alpha1Role>(WatchMaker, Lister, ct: ct);
            return source;
        }

        private EventSource<V1beta1RoleBinding> V1beta1RoleBinding(IKubernetes operations, string @namespace, CancellationToken ct)
        {
            Watcher<V1beta1RoleBinding> WatchMaker(EventSourceHandler onEvent, Action<Exception> onError, Action onClosed)
            {
                var list = operations.ListNamespacedRoleBinding2WithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1beta1RoleBinding obj) =>
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

            async Task<IList<V1beta1RoleBinding>> Lister()
            {
                var list = await operations.ListNamespacedRoleBinding2Async(@namespace);
                var kind = list.Kind.Replace("List", "");
                foreach (var i in list.Items)
                {
                    i.ApiVersion = list.ApiVersion;
                    i.Kind = kind;
                }
                return list.Items;
            }

            var source = new EventSource<V1beta1RoleBinding>(WatchMaker, Lister, ct: ct);
            return source;
        }

        private EventSource<V1beta1Role> V1beta1Role(IKubernetes operations, string @namespace, CancellationToken ct)
        {
            Watcher<V1beta1Role> WatchMaker(EventSourceHandler onEvent, Action<Exception> onError, Action onClosed)
            {
                var list = operations.ListNamespacedRole2WithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1beta1Role obj) =>
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

            async Task<IList<V1beta1Role>> Lister()
            {
                var list = await operations.ListNamespacedRole2Async(@namespace);
                var kind = list.Kind.Replace("List", "");
                foreach (var i in list.Items)
                {
                    i.ApiVersion = list.ApiVersion;
                    i.Kind = kind;
                }
                return list.Items;
            }

            var source = new EventSource<V1beta1Role>(WatchMaker, Lister, ct: ct);
            return source;
        }

        private EventSource<V1alpha1PodPreset> V1alpha1PodPreset(IKubernetes operations, string @namespace, CancellationToken ct)
        {
            Watcher<V1alpha1PodPreset> WatchMaker(EventSourceHandler onEvent, Action<Exception> onError, Action onClosed)
            {
                var list = operations.ListNamespacedPodPresetWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1alpha1PodPreset obj) =>
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

            async Task<IList<V1alpha1PodPreset>> Lister()
            {
                var list = await operations.ListNamespacedPodPresetAsync(@namespace);
                var kind = list.Kind.Replace("List", "");
                foreach (var i in list.Items)
                {
                    i.ApiVersion = list.ApiVersion;
                    i.Kind = kind;
                }
                return list.Items;
            }

            var source = new EventSource<V1alpha1PodPreset>(WatchMaker, Lister, ct: ct);
            return source;
        }

        private EventSource<V1NetworkPolicy> V1NetworkPolicy(IKubernetes operations, string @namespace, CancellationToken ct)
        {
            Watcher<V1NetworkPolicy> WatchMaker(EventSourceHandler onEvent, Action<Exception> onError, Action onClosed)
            {
                var list = operations.ListNamespacedNetworkPolicyWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1NetworkPolicy obj) =>
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

            async Task<IList<V1NetworkPolicy>> Lister()
            {
                var list = await operations.ListNamespacedNetworkPolicyAsync(@namespace);
                var kind = list.Kind.Replace("List", "");
                foreach (var i in list.Items)
                {
                    i.ApiVersion = list.ApiVersion;
                    i.Kind = kind;
                }
                return list.Items;
            }

            var source = new EventSource<V1NetworkPolicy>(WatchMaker, Lister, ct: ct);
            return source;
        }

        private EventSource<Networkingv1beta1Ingress> Networkingv1beta1Ingress(IKubernetes operations, string @namespace, CancellationToken ct)
        {
            Watcher<Networkingv1beta1Ingress> WatchMaker(EventSourceHandler onEvent, Action<Exception> onError, Action onClosed)
            {
                var list = operations.ListNamespacedIngress1WithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, Networkingv1beta1Ingress obj) =>
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

            async Task<IList<Networkingv1beta1Ingress>> Lister()
            {
                var list = await operations.ListNamespacedIngress1Async(@namespace);
                var kind = list.Kind.Replace("List", "");
                foreach (var i in list.Items)
                {
                    i.ApiVersion = list.ApiVersion;
                    i.Kind = kind;
                }
                return list.Items;
            }

            var source = new EventSource<Networkingv1beta1Ingress>(WatchMaker, Lister, ct: ct);
            return source;
        }

        private EventSource<V1beta1PodDisruptionBudget> V1beta1PodDisruptionBudget(IKubernetes operations, string @namespace, CancellationToken ct)
        {
            Watcher<V1beta1PodDisruptionBudget> WatchMaker(EventSourceHandler onEvent, Action<Exception> onError, Action onClosed)
            {
                var list = operations.ListNamespacedPodDisruptionBudgetWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1beta1PodDisruptionBudget obj) =>
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

            async Task<IList<V1beta1PodDisruptionBudget>> Lister()
            {
                var list = await operations.ListNamespacedPodDisruptionBudgetAsync(@namespace);
                var kind = list.Kind.Replace("List", "");
                foreach (var i in list.Items)
                {
                    i.ApiVersion = list.ApiVersion;
                    i.Kind = kind;
                }
                return list.Items;
            }

            var source = new EventSource<V1beta1PodDisruptionBudget>(WatchMaker, Lister, ct: ct);
            return source;
        }

        private EventSource<V1RoleBinding> V1RoleBinding(IKubernetes operations, string @namespace, CancellationToken ct)
        {
            Watcher<V1RoleBinding> WatchMaker(EventSourceHandler onEvent, Action<Exception> onError, Action onClosed)
            {
                var list = operations.ListNamespacedRoleBindingWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1RoleBinding obj) =>
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

            async Task<IList<V1RoleBinding>> Lister()
            {
                var list = await operations.ListNamespacedRoleBindingAsync(@namespace);
                var kind = list.Kind.Replace("List", "");
                foreach (var i in list.Items)
                {
                    i.ApiVersion = list.ApiVersion;
                    i.Kind = kind;
                }
                return list.Items;
            }

            var source = new EventSource<V1RoleBinding>(WatchMaker, Lister, ct: ct);
            return source;
        }

        private EventSource<V1Role> V1Role(IKubernetes operations, string @namespace, CancellationToken ct)
        {
            Watcher<V1Role> WatchMaker(EventSourceHandler onEvent, Action<Exception> onError, Action onClosed)
            {
                var list = operations.ListNamespacedRoleWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1Role obj) =>
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

            async Task<IList<V1Role>> Lister()
            {
                var list = await operations.ListNamespacedRoleAsync(@namespace);
                var kind = list.Kind.Replace("List", "");
                foreach (var i in list.Items)
                {
                    i.ApiVersion = list.ApiVersion;
                    i.Kind = kind;
                }
                return list.Items;
            }

            var source = new EventSource<V1Role>(WatchMaker, Lister, ct: ct);
            return source;
        }

        private EventSource<V2beta2HorizontalPodAutoscaler> V2beta2HorizontalPodAutoscaler(IKubernetes operations, string @namespace, CancellationToken ct)
        {
            Watcher<V2beta2HorizontalPodAutoscaler> WatchMaker(EventSourceHandler onEvent, Action<Exception> onError, Action onClosed)
            {
                var list = operations.ListNamespacedHorizontalPodAutoscaler2WithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V2beta2HorizontalPodAutoscaler obj) =>
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

            async Task<IList<V2beta2HorizontalPodAutoscaler>> Lister()
            {
                var list = await operations.ListNamespacedHorizontalPodAutoscaler2Async(@namespace);
                var kind = list.Kind.Replace("List", "");
                foreach (var i in list.Items)
                {
                    i.ApiVersion = list.ApiVersion;
                    i.Kind = kind;
                }
                return list.Items;
            }

            var source = new EventSource<V2beta2HorizontalPodAutoscaler>(WatchMaker, Lister, ct: ct);
            return source;
        }

        private EventSource<V1Job> V1Job(IKubernetes operations, string @namespace, CancellationToken ct)
        {
            Watcher<V1Job> WatchMaker(EventSourceHandler onEvent, Action<Exception> onError, Action onClosed)
            {
                var list = operations.ListNamespacedJobWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1Job obj) =>
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

            async Task<IList<V1Job>> Lister()
            {
                var list = await operations.ListNamespacedJobAsync(@namespace);
                var kind = list.Kind.Replace("List", "");
                foreach (var i in list.Items)
                {
                    i.ApiVersion = list.ApiVersion;
                    i.Kind = kind;
                }
                return list.Items;
            }

            var source = new EventSource<V1Job>(WatchMaker, Lister, ct: ct);
            return source;
        }

        private EventSource<V1beta1CronJob> V1beta1CronJob(IKubernetes operations, string @namespace, CancellationToken ct)
        {
            Watcher<V1beta1CronJob> WatchMaker(EventSourceHandler onEvent, Action<Exception> onError, Action onClosed)
            {
                var list = operations.ListNamespacedCronJobWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1beta1CronJob obj) =>
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

            async Task<IList<V1beta1CronJob>> Lister()
            {
                var list = await operations.ListNamespacedCronJobAsync(@namespace);
                var kind = list.Kind.Replace("List", "");
                foreach (var i in list.Items)
                {
                    i.ApiVersion = list.ApiVersion;
                    i.Kind = kind;
                }
                return list.Items;
            }

            var source = new EventSource<V1beta1CronJob>(WatchMaker, Lister, ct: ct);
            return source;
        }

        private EventSource<V2alpha1CronJob> V2alpha1CronJob(IKubernetes operations, string @namespace, CancellationToken ct)
        {
            Watcher<V2alpha1CronJob> WatchMaker(EventSourceHandler onEvent, Action<Exception> onError, Action onClosed)
            {
                var list = operations.ListNamespacedCronJob1WithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V2alpha1CronJob obj) =>
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

            async Task<IList<V2alpha1CronJob>> Lister()
            {
                var list = await operations.ListNamespacedCronJob1Async(@namespace);
                var kind = list.Kind.Replace("List", "");
                foreach (var i in list.Items)
                {
                    i.ApiVersion = list.ApiVersion;
                    i.Kind = kind;
                }
                return list.Items;
            }

            var source = new EventSource<V2alpha1CronJob>(WatchMaker, Lister, ct: ct);
            return source;
        }

        private EventSource<V1Lease> V1Lease(IKubernetes operations, string @namespace, CancellationToken ct)
        {
            Watcher<V1Lease> WatchMaker(EventSourceHandler onEvent, Action<Exception> onError, Action onClosed)
            {
                var list = operations.ListNamespacedLeaseWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1Lease obj) =>
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

            async Task<IList<V1Lease>> Lister()
            {
                var list = await operations.ListNamespacedLeaseAsync(@namespace);
                var kind = list.Kind.Replace("List", "");
                foreach (var i in list.Items)
                {
                    i.ApiVersion = list.ApiVersion;
                    i.Kind = kind;
                }
                return list.Items;
            }

            var source = new EventSource<V1Lease>(WatchMaker, Lister, ct: ct);
            return source;
        }

        private EventSource<V1beta1Lease> V1beta1Lease(IKubernetes operations, string @namespace, CancellationToken ct)
        {
            Watcher<V1beta1Lease> WatchMaker(EventSourceHandler onEvent, Action<Exception> onError, Action onClosed)
            {
                var list = operations.ListNamespacedLease1WithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1beta1Lease obj) =>
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

            async Task<IList<V1beta1Lease>> Lister()
            {
                var list = await operations.ListNamespacedLease1Async(@namespace);
                var kind = list.Kind.Replace("List", "");
                foreach (var i in list.Items)
                {
                    i.ApiVersion = list.ApiVersion;
                    i.Kind = kind;
                }
                return list.Items;
            }

            var source = new EventSource<V1beta1Lease>(WatchMaker, Lister, ct: ct);
            return source;
        }

        private EventSource<V1beta1EndpointSlice> V1beta1EndpointSlice(IKubernetes operations, string @namespace, CancellationToken ct)
        {
            Watcher<V1beta1EndpointSlice> WatchMaker(EventSourceHandler onEvent, Action<Exception> onError, Action onClosed)
            {
                var list = operations.ListNamespacedEndpointSliceWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1beta1EndpointSlice obj) =>
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

            async Task<IList<V1beta1EndpointSlice>> Lister()
            {
                var list = await operations.ListNamespacedEndpointSliceAsync(@namespace);
                var kind = list.Kind.Replace("List", "");
                foreach (var i in list.Items)
                {
                    i.ApiVersion = list.ApiVersion;
                    i.Kind = kind;
                }
                return list.Items;
            }

            var source = new EventSource<V1beta1EndpointSlice>(WatchMaker, Lister, ct: ct);
            return source;
        }

        private EventSource<V1beta1Event> V1beta1Event(IKubernetes operations, string @namespace, CancellationToken ct)
        {
            Watcher<V1beta1Event> WatchMaker(EventSourceHandler onEvent, Action<Exception> onError, Action onClosed)
            {
                var list = operations.ListNamespacedEvent1WithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1beta1Event obj) =>
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

            async Task<IList<V1beta1Event>> Lister()
            {
                var list = await operations.ListNamespacedEvent1Async(@namespace);
                var kind = list.Kind.Replace("List", "");
                foreach (var i in list.Items)
                {
                    i.ApiVersion = list.ApiVersion;
                    i.Kind = kind;
                }
                return list.Items;
            }

            var source = new EventSource<V1beta1Event>(WatchMaker, Lister, ct: ct);
            return source;
        }

        private EventSource<Extensionsv1beta1Ingress> Extensionsv1beta1Ingress(IKubernetes operations, string @namespace, CancellationToken ct)
        {
            Watcher<Extensionsv1beta1Ingress> WatchMaker(EventSourceHandler onEvent, Action<Exception> onError, Action onClosed)
            {
                var list = operations.ListNamespacedIngressWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, Extensionsv1beta1Ingress obj) =>
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

            async Task<IList<Extensionsv1beta1Ingress>> Lister()
            {
                var list = await operations.ListNamespacedIngressAsync(@namespace);
                var kind = list.Kind.Replace("List", "");
                foreach (var i in list.Items)
                {
                    i.ApiVersion = list.ApiVersion;
                    i.Kind = kind;
                }
                return list.Items;
            }

            var source = new EventSource<Extensionsv1beta1Ingress>(WatchMaker, Lister, ct: ct);
            return source;
        }

        private EventSource<V1ControllerRevision> V1ControllerRevision(IKubernetes operations, string @namespace, CancellationToken ct)
        {
            Watcher<V1ControllerRevision> WatchMaker(EventSourceHandler onEvent, Action<Exception> onError, Action onClosed)
            {
                var list = operations.ListNamespacedControllerRevisionWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1ControllerRevision obj) =>
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

            async Task<IList<V1ControllerRevision>> Lister()
            {
                var list = await operations.ListNamespacedControllerRevisionAsync(@namespace);
                var kind = list.Kind.Replace("List", "");
                foreach (var i in list.Items)
                {
                    i.ApiVersion = list.ApiVersion;
                    i.Kind = kind;
                }
                return list.Items;
            }

            var source = new EventSource<V1ControllerRevision>(WatchMaker, Lister, ct: ct);
            return source;
        }

        private EventSource<V1DaemonSet> V1DaemonSet(IKubernetes operations, string @namespace, CancellationToken ct)
        {
            Watcher<V1DaemonSet> WatchMaker(EventSourceHandler onEvent, Action<Exception> onError, Action onClosed)
            {
                var list = operations.ListNamespacedDaemonSetWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1DaemonSet obj) =>
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

            async Task<IList<V1DaemonSet>> Lister()
            {
                var list = await operations.ListNamespacedDaemonSetAsync(@namespace);
                var kind = list.Kind.Replace("List", "");
                foreach (var i in list.Items)
                {
                    i.ApiVersion = list.ApiVersion;
                    i.Kind = kind;
                }
                return list.Items;
            }

            var source = new EventSource<V1DaemonSet>(WatchMaker, Lister, ct: ct);
            return source;
        }

        private EventSource<V1Deployment> V1Deployment(IKubernetes operations, string @namespace, CancellationToken ct)
        {
            Watcher<V1Deployment> WatchMaker(EventSourceHandler onEvent, Action<Exception> onError, Action onClosed)
            {
                var list = operations.ListNamespacedDeploymentWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1Deployment obj) =>
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

            async Task<IList<V1Deployment>> Lister()
            {
                var list = await operations.ListNamespacedDeploymentAsync(@namespace);
                var kind = list.Kind.Replace("List", "");
                foreach (var i in list.Items)
                {
                    i.ApiVersion = list.ApiVersion;
                    i.Kind = kind;
                }
                return list.Items;
            }

            var source = new EventSource<V1Deployment>(WatchMaker, Lister, ct: ct);
            return source;
        }

        private EventSource<V1ReplicaSet> V1ReplicaSet(IKubernetes operations, string @namespace, CancellationToken ct)
        {
            Watcher<V1ReplicaSet> WatchMaker(EventSourceHandler onEvent, Action<Exception> onError, Action onClosed)
            {
                var list = operations.ListNamespacedReplicaSetWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1ReplicaSet obj) =>
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

            async Task<IList<V1ReplicaSet>> Lister()
            {
                var list = await operations.ListNamespacedReplicaSetAsync(@namespace);
                var kind = list.Kind.Replace("List", "");
                foreach (var i in list.Items)
                {
                    i.ApiVersion = list.ApiVersion;
                    i.Kind = kind;
                }
                return list.Items;
            }

            var source = new EventSource<V1ReplicaSet>(WatchMaker, Lister, ct: ct);
            return source;
        }

        private EventSource<V1StatefulSet> V1StatefulSet(IKubernetes operations, string @namespace, CancellationToken ct)
        {
            Watcher<V1StatefulSet> WatchMaker(EventSourceHandler onEvent, Action<Exception> onError, Action onClosed)
            {
                var list = operations.ListNamespacedStatefulSetWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1StatefulSet obj) =>
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

            async Task<IList<V1StatefulSet>> Lister()
            {
                var list = await operations.ListNamespacedStatefulSetAsync(@namespace);
                var kind = list.Kind.Replace("List", "");
                foreach (var i in list.Items)
                {
                    i.ApiVersion = list.ApiVersion;
                    i.Kind = kind;
                }
                return list.Items;
            }

            var source = new EventSource<V1StatefulSet>(WatchMaker, Lister, ct: ct);
            return source;
        }

        private EventSource<V1HorizontalPodAutoscaler> V1HorizontalPodAutoscaler(IKubernetes operations, string @namespace, CancellationToken ct)
        {
            Watcher<V1HorizontalPodAutoscaler> WatchMaker(EventSourceHandler onEvent, Action<Exception> onError, Action onClosed)
            {
                var list = operations.ListNamespacedHorizontalPodAutoscalerWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1HorizontalPodAutoscaler obj) =>
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

            async Task<IList<V1HorizontalPodAutoscaler>> Lister()
            {
                var list = await operations.ListNamespacedHorizontalPodAutoscalerAsync(@namespace);
                var kind = list.Kind.Replace("List", "");
                foreach (var i in list.Items)
                {
                    i.ApiVersion = list.ApiVersion;
                    i.Kind = kind;
                }
                return list.Items;
            }

            var source = new EventSource<V1HorizontalPodAutoscaler>(WatchMaker, Lister, ct: ct);
            return source;
        }

        private EventSource<V2beta1HorizontalPodAutoscaler> V2beta1HorizontalPodAutoscaler(IKubernetes operations, string @namespace, CancellationToken ct)
        {
            Watcher<V2beta1HorizontalPodAutoscaler> WatchMaker(EventSourceHandler onEvent, Action<Exception> onError, Action onClosed)
            {
                var list = operations.ListNamespacedHorizontalPodAutoscaler1WithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V2beta1HorizontalPodAutoscaler obj) =>
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

            async Task<IList<V2beta1HorizontalPodAutoscaler>> Lister()
            {
                var list = await operations.ListNamespacedHorizontalPodAutoscaler1Async(@namespace);
                var kind = list.Kind.Replace("List", "");
                foreach (var i in list.Items)
                {
                    i.ApiVersion = list.ApiVersion;
                    i.Kind = kind;
                }
                return list.Items;
            }

            var source = new EventSource<V2beta1HorizontalPodAutoscaler>(WatchMaker, Lister, ct: ct);
            return source;
        }

        private EventSource<V1Service> V1Service(IKubernetes operations, string @namespace, CancellationToken ct)
        {
            Watcher<V1Service> WatchMaker(EventSourceHandler onEvent, Action<Exception> onError, Action onClosed)
            {
                var list = operations.ListNamespacedServiceWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1Service obj) =>
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

            async Task<IList<V1Service>> Lister()
            {
                var list = await operations.ListNamespacedServiceAsync(@namespace);
                var kind = list.Kind.Replace("List", "");
                foreach (var i in list.Items)
                {
                    i.ApiVersion = list.ApiVersion;
                    i.Kind = kind;
                }
                return list.Items;
            }

            var source = new EventSource<V1Service>(WatchMaker, Lister, ct: ct);
            return source;
        }

        private EventSource<V1ConfigMap> V1ConfigMap(IKubernetes operations, string @namespace, CancellationToken ct)
        {
            Watcher<V1ConfigMap> WatchMaker(EventSourceHandler onEvent, Action<Exception> onError, Action onClosed)
            {
                var list = operations.ListNamespacedConfigMapWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1ConfigMap obj) =>
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

            async Task<IList<V1ConfigMap>> Lister()
            {
                var list = await operations.ListNamespacedConfigMapAsync(@namespace);
                var kind = list.Kind.Replace("List", "");
                foreach (var i in list.Items)
                {
                    i.ApiVersion = list.ApiVersion;
                    i.Kind = kind;
                }
                return list.Items;
            }

            var source = new EventSource<V1ConfigMap>(WatchMaker, Lister, ct: ct);
            return source;
        }

        private EventSource<V1Endpoints> V1Endpoints(IKubernetes operations, string @namespace, CancellationToken ct)
        {
            Watcher<V1Endpoints> WatchMaker(EventSourceHandler onEvent, Action<Exception> onError, Action onClosed)
            {
                var list = operations.ListNamespacedEndpointsWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1Endpoints obj) =>
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

            async Task<IList<V1Endpoints>> Lister()
            {
                var list = await operations.ListNamespacedEndpointsAsync(@namespace);
                var kind = list.Kind.Replace("List", "");
                foreach (var i in list.Items)
                {
                    i.ApiVersion = list.ApiVersion;
                    i.Kind = kind;
                }
                return list.Items;
            }

            var source = new EventSource<V1Endpoints>(WatchMaker, Lister, ct: ct);
            return source;
        }

        private EventSource<V1Event> V1Event(IKubernetes operations, string @namespace, CancellationToken ct)
        {
            Watcher<V1Event> WatchMaker(EventSourceHandler onEvent, Action<Exception> onError, Action onClosed)
            {
                var list = operations.ListNamespacedEventWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1Event obj) =>
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

            async Task<IList<V1Event>> Lister()
            {
                var list = await operations.ListNamespacedEventAsync(@namespace);
                var kind = list.Kind.Replace("List", "");
                foreach (var i in list.Items)
                {
                    i.ApiVersion = list.ApiVersion;
                    i.Kind = kind;
                }
                return list.Items;
            }

            var source = new EventSource<V1Event>(WatchMaker, Lister, ct: ct);
            return source;
        }

        private EventSource<V1LimitRange> V1LimitRange(IKubernetes operations, string @namespace, CancellationToken ct)
        {
            Watcher<V1LimitRange> WatchMaker(EventSourceHandler onEvent, Action<Exception> onError, Action onClosed)
            {
                var list = operations.ListNamespacedLimitRangeWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1LimitRange obj) =>
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

            async Task<IList<V1LimitRange>> Lister()
            {
                var list = await operations.ListNamespacedLimitRangeAsync(@namespace);
                var kind = list.Kind.Replace("List", "");
                foreach (var i in list.Items)
                {
                    i.ApiVersion = list.ApiVersion;
                    i.Kind = kind;
                }
                return list.Items;
            }

            var source = new EventSource<V1LimitRange>(WatchMaker, Lister, ct: ct);
            return source;
        }

        private EventSource<V1PersistentVolumeClaim> V1PersistentVolumeClaim(IKubernetes operations, string @namespace, CancellationToken ct)
        {
            Watcher<V1PersistentVolumeClaim> WatchMaker(EventSourceHandler onEvent, Action<Exception> onError, Action onClosed)
            {
                var list = operations.ListNamespacedPersistentVolumeClaimWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1PersistentVolumeClaim obj) =>
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

            async Task<IList<V1PersistentVolumeClaim>> Lister()
            {
                var list = await operations.ListNamespacedPersistentVolumeClaimAsync(@namespace);
                var kind = list.Kind.Replace("List", "");
                foreach (var i in list.Items)
                {
                    i.ApiVersion = list.ApiVersion;
                    i.Kind = kind;
                }
                return list.Items;
            }

            var source = new EventSource<V1PersistentVolumeClaim>(WatchMaker, Lister, ct: ct);
            return source;
        }

        private EventSource<V1Pod> V1Pod(IKubernetes operations, string @namespace, CancellationToken ct)
        {
            Watcher<V1Pod> WatchMaker(EventSourceHandler onEvent, Action<Exception> onError, Action onClosed)
            {
                var list = operations.ListNamespacedPodWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1Pod obj) =>
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

            async Task<IList<V1Pod>> Lister()
            {
                var list = await operations.ListNamespacedPodAsync(@namespace);
                var kind = list.Kind.Replace("List", "");
                foreach (var i in list.Items)
                {
                    i.ApiVersion = list.ApiVersion;
                    i.Kind = kind;
                }
                return list.Items;
            }

            var source = new EventSource<V1Pod>(WatchMaker, Lister, ct: ct);
            return source;
        }

        private EventSource<V1PodTemplate> V1PodTemplate(IKubernetes operations, string @namespace, CancellationToken ct)
        {
            Watcher<V1PodTemplate> WatchMaker(EventSourceHandler onEvent, Action<Exception> onError, Action onClosed)
            {
                var list = operations.ListNamespacedPodTemplateWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1PodTemplate obj) =>
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

            async Task<IList<V1PodTemplate>> Lister()
            {
                var list = await operations.ListNamespacedPodTemplateAsync(@namespace);
                var kind = list.Kind.Replace("List", "");
                foreach (var i in list.Items)
                {
                    i.ApiVersion = list.ApiVersion;
                    i.Kind = kind;
                }
                return list.Items;
            }

            var source = new EventSource<V1PodTemplate>(WatchMaker, Lister, ct: ct);
            return source;
        }

        private EventSource<V1ReplicationController> V1ReplicationController(IKubernetes operations, string @namespace, CancellationToken ct)
        {
            Watcher<V1ReplicationController> WatchMaker(EventSourceHandler onEvent, Action<Exception> onError, Action onClosed)
            {
                var list = operations.ListNamespacedReplicationControllerWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1ReplicationController obj) =>
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

            async Task<IList<V1ReplicationController>> Lister()
            {
                var list = await operations.ListNamespacedReplicationControllerAsync(@namespace);
                var kind = list.Kind.Replace("List", "");
                foreach (var i in list.Items)
                {
                    i.ApiVersion = list.ApiVersion;
                    i.Kind = kind;
                }
                return list.Items;
            }

            var source = new EventSource<V1ReplicationController>(WatchMaker, Lister, ct: ct);
            return source;
        }

        private EventSource<V1ResourceQuota> V1ResourceQuota(IKubernetes operations, string @namespace, CancellationToken ct)
        {
            Watcher<V1ResourceQuota> WatchMaker(EventSourceHandler onEvent, Action<Exception> onError, Action onClosed)
            {
                var list = operations.ListNamespacedResourceQuotaWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1ResourceQuota obj) =>
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

            async Task<IList<V1ResourceQuota>> Lister()
            {
                var list = await operations.ListNamespacedResourceQuotaAsync(@namespace);
                var kind = list.Kind.Replace("List", "");
                foreach (var i in list.Items)
                {
                    i.ApiVersion = list.ApiVersion;
                    i.Kind = kind;
                }
                return list.Items;
            }

            var source = new EventSource<V1ResourceQuota>(WatchMaker, Lister, ct: ct);
            return source;
        }

        private EventSource<V1Secret> V1Secret(IKubernetes operations, string @namespace, CancellationToken ct)
        {
            Watcher<V1Secret> WatchMaker(EventSourceHandler onEvent, Action<Exception> onError, Action onClosed)
            {
                var list = operations.ListNamespacedSecretWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1Secret obj) =>
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

            async Task<IList<V1Secret>> Lister()
            {
                var list = await operations.ListNamespacedSecretAsync(@namespace);
                var kind = list.Kind.Replace("List", "");
                foreach (var i in list.Items)
                {
                    i.ApiVersion = list.ApiVersion;
                    i.Kind = kind;
                }
                return list.Items;
            }

            var source = new EventSource<V1Secret>(WatchMaker, Lister, ct: ct);
            return source;
        }

        private EventSource<V1ServiceAccount> V1ServiceAccount(IKubernetes operations, string @namespace, CancellationToken ct)
        {
            Watcher<V1ServiceAccount> WatchMaker(EventSourceHandler onEvent, Action<Exception> onError, Action onClosed)
            {
                var list = operations.ListNamespacedServiceAccountWithHttpMessagesAsync(@namespace, watch: true);
                var watch = list.Watch(async (WatchEventType et, V1ServiceAccount obj) =>
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

            async Task<IList<V1ServiceAccount>> Lister()
            {
                var list = await operations.ListNamespacedServiceAccountAsync(@namespace);
                var kind = list.Kind.Replace("List", "");
                foreach (var i in list.Items)
                {
                    i.ApiVersion = list.ApiVersion;
                    i.Kind = kind;
                }
                return list.Items;
            }

            var source = new EventSource<V1ServiceAccount>(WatchMaker, Lister, ct: ct);
            return source;
        }
    }
}
