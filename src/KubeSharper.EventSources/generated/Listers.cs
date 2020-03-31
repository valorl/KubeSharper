using k8s;
using k8s.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KubeSharper.EventSources
{
    public partial class Listers
    {
        public Lister<T> GetNamespacedFor<T>(IKubernetes operations, string @namespace)
        {
            var t = typeof(T);
            if (t == typeof(V1alpha1PodPreset)) return (Lister<T>)(object)V1alpha1PodPreset(operations);
            else if (t == typeof(V1alpha1Role)) return (Lister<T>)(object)V1alpha1Role(operations);
            else if (t == typeof(V1beta1RoleBinding)) return (Lister<T>)(object)V1beta1RoleBinding(operations);
            else if (t == typeof(V1beta1Role)) return (Lister<T>)(object)V1beta1Role(operations);
            else if (t == typeof(V1RoleBinding)) return (Lister<T>)(object)V1RoleBinding(operations);
            else if (t == typeof(V1Role)) return (Lister<T>)(object)V1Role(operations);
            else if (t == typeof(V1alpha1RoleBinding)) return (Lister<T>)(object)V1alpha1RoleBinding(operations);
            else if (t == typeof(V1beta1PodDisruptionBudget)) return (Lister<T>)(object)V1beta1PodDisruptionBudget(operations);
            else if (t == typeof(V1NetworkPolicy)) return (Lister<T>)(object)V1NetworkPolicy(operations);
            else if (t == typeof(Networkingv1beta1Ingress)) return (Lister<T>)(object)Networkingv1beta1Ingress(operations);
            else if (t == typeof(Extensionsv1beta1Deployment)) return (Lister<T>)(object)Extensionsv1beta1Deployment(operations);
            else if (t == typeof(Extensionsv1beta1Ingress)) return (Lister<T>)(object)Extensionsv1beta1Ingress(operations);
            else if (t == typeof(V1beta1NetworkPolicy)) return (Lister<T>)(object)V1beta1NetworkPolicy(operations);
            else if (t == typeof(V1beta1ReplicaSet)) return (Lister<T>)(object)V1beta1ReplicaSet(operations);
            else if (t == typeof(V1beta1Lease)) return (Lister<T>)(object)V1beta1Lease(operations);
            else if (t == typeof(V1alpha1EndpointSlice)) return (Lister<T>)(object)V1alpha1EndpointSlice(operations);
            else if (t == typeof(V1beta1Event)) return (Lister<T>)(object)V1beta1Event(operations);
            else if (t == typeof(V1beta1DaemonSet)) return (Lister<T>)(object)V1beta1DaemonSet(operations);
            else if (t == typeof(V1beta1CronJob)) return (Lister<T>)(object)V1beta1CronJob(operations);
            else if (t == typeof(V2alpha1CronJob)) return (Lister<T>)(object)V2alpha1CronJob(operations);
            else if (t == typeof(V1Lease)) return (Lister<T>)(object)V1Lease(operations);
            else if (t == typeof(V2beta1HorizontalPodAutoscaler)) return (Lister<T>)(object)V2beta1HorizontalPodAutoscaler(operations);
            else if (t == typeof(V2beta2HorizontalPodAutoscaler)) return (Lister<T>)(object)V2beta2HorizontalPodAutoscaler(operations);
            else if (t == typeof(V1Job)) return (Lister<T>)(object)V1Job(operations);
            else if (t == typeof(V1HorizontalPodAutoscaler)) return (Lister<T>)(object)V1HorizontalPodAutoscaler(operations);
            else if (t == typeof(V1beta2DaemonSet)) return (Lister<T>)(object)V1beta2DaemonSet(operations);
            else if (t == typeof(V1beta2Deployment)) return (Lister<T>)(object)V1beta2Deployment(operations);
            else if (t == typeof(V1beta2ReplicaSet)) return (Lister<T>)(object)V1beta2ReplicaSet(operations);
            else if (t == typeof(V1beta2StatefulSet)) return (Lister<T>)(object)V1beta2StatefulSet(operations);
            else if (t == typeof(Appsv1beta1Deployment)) return (Lister<T>)(object)Appsv1beta1Deployment(operations);
            else if (t == typeof(V1beta1StatefulSet)) return (Lister<T>)(object)V1beta1StatefulSet(operations);
            else if (t == typeof(V1beta2ControllerRevision)) return (Lister<T>)(object)V1beta2ControllerRevision(operations);
            else if (t == typeof(V1ReplicaSet)) return (Lister<T>)(object)V1ReplicaSet(operations);
            else if (t == typeof(V1StatefulSet)) return (Lister<T>)(object)V1StatefulSet(operations);
            else if (t == typeof(V1beta1ControllerRevision)) return (Lister<T>)(object)V1beta1ControllerRevision(operations);
            else if (t == typeof(V1ControllerRevision)) return (Lister<T>)(object)V1ControllerRevision(operations);
            else if (t == typeof(V1DaemonSet)) return (Lister<T>)(object)V1DaemonSet(operations);
            else if (t == typeof(V1Deployment)) return (Lister<T>)(object)V1Deployment(operations);
            else if (t == typeof(V1ReplicationController)) return (Lister<T>)(object)V1ReplicationController(operations);
            else if (t == typeof(V1ResourceQuota)) return (Lister<T>)(object)V1ResourceQuota(operations);
            else if (t == typeof(V1Secret)) return (Lister<T>)(object)V1Secret(operations);
            else if (t == typeof(V1ServiceAccount)) return (Lister<T>)(object)V1ServiceAccount(operations);
            else if (t == typeof(V1Service)) return (Lister<T>)(object)V1Service(operations);
            else if (t == typeof(V1Pod)) return (Lister<T>)(object)V1Pod(operations);
            else if (t == typeof(V1PodTemplate)) return (Lister<T>)(object)V1PodTemplate(operations);
            else if (t == typeof(V1ConfigMap)) return (Lister<T>)(object)V1ConfigMap(operations);
            else if (t == typeof(V1Endpoints)) return (Lister<T>)(object)V1Endpoints(operations);
            else if (t == typeof(V1Event)) return (Lister<T>)(object)V1Event(operations);
            else if (t == typeof(V1LimitRange)) return (Lister<T>)(object)V1LimitRange(operations);
            else if (t == typeof(V1PersistentVolumeClaim)) return (Lister<T>)(object)V1PersistentVolumeClaim(operations);
            else throw new NotImplementedException();
        }


        public Lister<V1alpha1PodPreset> V1alpha1PodPreset(IKubernetes operations)
        {
            async Task<IList<V1alpha1PodPreset>> ListerFunc(string @namespace)
            {
                var list = await operations.ListNamespacedPodPresetAsync(@namespace);
                return list.Items;
            }
            return new Lister<V1alpha1PodPreset>(ListerFunc);
        }

        public Lister<V1alpha1Role> V1alpha1Role(IKubernetes operations)
        {
            async Task<IList<V1alpha1Role>> ListerFunc(string @namespace)
            {
                var list = await operations.ListNamespacedRole1Async(@namespace);
                return list.Items;
            }
            return new Lister<V1alpha1Role>(ListerFunc);
        }

        public Lister<V1beta1RoleBinding> V1beta1RoleBinding(IKubernetes operations)
        {
            async Task<IList<V1beta1RoleBinding>> ListerFunc(string @namespace)
            {
                var list = await operations.ListNamespacedRoleBinding2Async(@namespace);
                return list.Items;
            }
            return new Lister<V1beta1RoleBinding>(ListerFunc);
        }

        public Lister<V1beta1Role> V1beta1Role(IKubernetes operations)
        {
            async Task<IList<V1beta1Role>> ListerFunc(string @namespace)
            {
                var list = await operations.ListNamespacedRole2Async(@namespace);
                return list.Items;
            }
            return new Lister<V1beta1Role>(ListerFunc);
        }

        public Lister<V1RoleBinding> V1RoleBinding(IKubernetes operations)
        {
            async Task<IList<V1RoleBinding>> ListerFunc(string @namespace)
            {
                var list = await operations.ListNamespacedRoleBindingAsync(@namespace);
                return list.Items;
            }
            return new Lister<V1RoleBinding>(ListerFunc);
        }

        public Lister<V1Role> V1Role(IKubernetes operations)
        {
            async Task<IList<V1Role>> ListerFunc(string @namespace)
            {
                var list = await operations.ListNamespacedRoleAsync(@namespace);
                return list.Items;
            }
            return new Lister<V1Role>(ListerFunc);
        }

        public Lister<V1alpha1RoleBinding> V1alpha1RoleBinding(IKubernetes operations)
        {
            async Task<IList<V1alpha1RoleBinding>> ListerFunc(string @namespace)
            {
                var list = await operations.ListNamespacedRoleBinding1Async(@namespace);
                return list.Items;
            }
            return new Lister<V1alpha1RoleBinding>(ListerFunc);
        }

        public Lister<V1beta1PodDisruptionBudget> V1beta1PodDisruptionBudget(IKubernetes operations)
        {
            async Task<IList<V1beta1PodDisruptionBudget>> ListerFunc(string @namespace)
            {
                var list = await operations.ListNamespacedPodDisruptionBudgetAsync(@namespace);
                return list.Items;
            }
            return new Lister<V1beta1PodDisruptionBudget>(ListerFunc);
        }

        public Lister<V1NetworkPolicy> V1NetworkPolicy(IKubernetes operations)
        {
            async Task<IList<V1NetworkPolicy>> ListerFunc(string @namespace)
            {
                var list = await operations.ListNamespacedNetworkPolicy1Async(@namespace);
                return list.Items;
            }
            return new Lister<V1NetworkPolicy>(ListerFunc);
        }

        public Lister<Networkingv1beta1Ingress> Networkingv1beta1Ingress(IKubernetes operations)
        {
            async Task<IList<Networkingv1beta1Ingress>> ListerFunc(string @namespace)
            {
                var list = await operations.ListNamespacedIngress1Async(@namespace);
                return list.Items;
            }
            return new Lister<Networkingv1beta1Ingress>(ListerFunc);
        }

        public Lister<Extensionsv1beta1Deployment> Extensionsv1beta1Deployment(IKubernetes operations)
        {
            async Task<IList<Extensionsv1beta1Deployment>> ListerFunc(string @namespace)
            {
                var list = await operations.ListNamespacedDeployment3Async(@namespace);
                return list.Items;
            }
            return new Lister<Extensionsv1beta1Deployment>(ListerFunc);
        }

        public Lister<Extensionsv1beta1Ingress> Extensionsv1beta1Ingress(IKubernetes operations)
        {
            async Task<IList<Extensionsv1beta1Ingress>> ListerFunc(string @namespace)
            {
                var list = await operations.ListNamespacedIngressAsync(@namespace);
                return list.Items;
            }
            return new Lister<Extensionsv1beta1Ingress>(ListerFunc);
        }

        public Lister<V1beta1NetworkPolicy> V1beta1NetworkPolicy(IKubernetes operations)
        {
            async Task<IList<V1beta1NetworkPolicy>> ListerFunc(string @namespace)
            {
                var list = await operations.ListNamespacedNetworkPolicyAsync(@namespace);
                return list.Items;
            }
            return new Lister<V1beta1NetworkPolicy>(ListerFunc);
        }

        public Lister<V1beta1ReplicaSet> V1beta1ReplicaSet(IKubernetes operations)
        {
            async Task<IList<V1beta1ReplicaSet>> ListerFunc(string @namespace)
            {
                var list = await operations.ListNamespacedReplicaSet2Async(@namespace);
                return list.Items;
            }
            return new Lister<V1beta1ReplicaSet>(ListerFunc);
        }

        public Lister<V1beta1Lease> V1beta1Lease(IKubernetes operations)
        {
            async Task<IList<V1beta1Lease>> ListerFunc(string @namespace)
            {
                var list = await operations.ListNamespacedLease1Async(@namespace);
                return list.Items;
            }
            return new Lister<V1beta1Lease>(ListerFunc);
        }

        public Lister<V1alpha1EndpointSlice> V1alpha1EndpointSlice(IKubernetes operations)
        {
            async Task<IList<V1alpha1EndpointSlice>> ListerFunc(string @namespace)
            {
                var list = await operations.ListNamespacedEndpointSliceAsync(@namespace);
                return list.Items;
            }
            return new Lister<V1alpha1EndpointSlice>(ListerFunc);
        }

        public Lister<V1beta1Event> V1beta1Event(IKubernetes operations)
        {
            async Task<IList<V1beta1Event>> ListerFunc(string @namespace)
            {
                var list = await operations.ListNamespacedEvent1Async(@namespace);
                return list.Items;
            }
            return new Lister<V1beta1Event>(ListerFunc);
        }

        public Lister<V1beta1DaemonSet> V1beta1DaemonSet(IKubernetes operations)
        {
            async Task<IList<V1beta1DaemonSet>> ListerFunc(string @namespace)
            {
                var list = await operations.ListNamespacedDaemonSet2Async(@namespace);
                return list.Items;
            }
            return new Lister<V1beta1DaemonSet>(ListerFunc);
        }

        public Lister<V1beta1CronJob> V1beta1CronJob(IKubernetes operations)
        {
            async Task<IList<V1beta1CronJob>> ListerFunc(string @namespace)
            {
                var list = await operations.ListNamespacedCronJobAsync(@namespace);
                return list.Items;
            }
            return new Lister<V1beta1CronJob>(ListerFunc);
        }

        public Lister<V2alpha1CronJob> V2alpha1CronJob(IKubernetes operations)
        {
            async Task<IList<V2alpha1CronJob>> ListerFunc(string @namespace)
            {
                var list = await operations.ListNamespacedCronJob1Async(@namespace);
                return list.Items;
            }
            return new Lister<V2alpha1CronJob>(ListerFunc);
        }

        public Lister<V1Lease> V1Lease(IKubernetes operations)
        {
            async Task<IList<V1Lease>> ListerFunc(string @namespace)
            {
                var list = await operations.ListNamespacedLeaseAsync(@namespace);
                return list.Items;
            }
            return new Lister<V1Lease>(ListerFunc);
        }

        public Lister<V2beta1HorizontalPodAutoscaler> V2beta1HorizontalPodAutoscaler(IKubernetes operations)
        {
            async Task<IList<V2beta1HorizontalPodAutoscaler>> ListerFunc(string @namespace)
            {
                var list = await operations.ListNamespacedHorizontalPodAutoscaler1Async(@namespace);
                return list.Items;
            }
            return new Lister<V2beta1HorizontalPodAutoscaler>(ListerFunc);
        }

        public Lister<V2beta2HorizontalPodAutoscaler> V2beta2HorizontalPodAutoscaler(IKubernetes operations)
        {
            async Task<IList<V2beta2HorizontalPodAutoscaler>> ListerFunc(string @namespace)
            {
                var list = await operations.ListNamespacedHorizontalPodAutoscaler2Async(@namespace);
                return list.Items;
            }
            return new Lister<V2beta2HorizontalPodAutoscaler>(ListerFunc);
        }

        public Lister<V1Job> V1Job(IKubernetes operations)
        {
            async Task<IList<V1Job>> ListerFunc(string @namespace)
            {
                var list = await operations.ListNamespacedJobAsync(@namespace);
                return list.Items;
            }
            return new Lister<V1Job>(ListerFunc);
        }

        public Lister<V1HorizontalPodAutoscaler> V1HorizontalPodAutoscaler(IKubernetes operations)
        {
            async Task<IList<V1HorizontalPodAutoscaler>> ListerFunc(string @namespace)
            {
                var list = await operations.ListNamespacedHorizontalPodAutoscalerAsync(@namespace);
                return list.Items;
            }
            return new Lister<V1HorizontalPodAutoscaler>(ListerFunc);
        }

        public Lister<V1beta2DaemonSet> V1beta2DaemonSet(IKubernetes operations)
        {
            async Task<IList<V1beta2DaemonSet>> ListerFunc(string @namespace)
            {
                var list = await operations.ListNamespacedDaemonSet1Async(@namespace);
                return list.Items;
            }
            return new Lister<V1beta2DaemonSet>(ListerFunc);
        }

        public Lister<V1beta2Deployment> V1beta2Deployment(IKubernetes operations)
        {
            async Task<IList<V1beta2Deployment>> ListerFunc(string @namespace)
            {
                var list = await operations.ListNamespacedDeployment2Async(@namespace);
                return list.Items;
            }
            return new Lister<V1beta2Deployment>(ListerFunc);
        }

        public Lister<V1beta2ReplicaSet> V1beta2ReplicaSet(IKubernetes operations)
        {
            async Task<IList<V1beta2ReplicaSet>> ListerFunc(string @namespace)
            {
                var list = await operations.ListNamespacedReplicaSet1Async(@namespace);
                return list.Items;
            }
            return new Lister<V1beta2ReplicaSet>(ListerFunc);
        }

        public Lister<V1beta2StatefulSet> V1beta2StatefulSet(IKubernetes operations)
        {
            async Task<IList<V1beta2StatefulSet>> ListerFunc(string @namespace)
            {
                var list = await operations.ListNamespacedStatefulSet2Async(@namespace);
                return list.Items;
            }
            return new Lister<V1beta2StatefulSet>(ListerFunc);
        }

        public Lister<Appsv1beta1Deployment> Appsv1beta1Deployment(IKubernetes operations)
        {
            async Task<IList<Appsv1beta1Deployment>> ListerFunc(string @namespace)
            {
                var list = await operations.ListNamespacedDeployment1Async(@namespace);
                return list.Items;
            }
            return new Lister<Appsv1beta1Deployment>(ListerFunc);
        }

        public Lister<V1beta1StatefulSet> V1beta1StatefulSet(IKubernetes operations)
        {
            async Task<IList<V1beta1StatefulSet>> ListerFunc(string @namespace)
            {
                var list = await operations.ListNamespacedStatefulSet1Async(@namespace);
                return list.Items;
            }
            return new Lister<V1beta1StatefulSet>(ListerFunc);
        }

        public Lister<V1beta2ControllerRevision> V1beta2ControllerRevision(IKubernetes operations)
        {
            async Task<IList<V1beta2ControllerRevision>> ListerFunc(string @namespace)
            {
                var list = await operations.ListNamespacedControllerRevision2Async(@namespace);
                return list.Items;
            }
            return new Lister<V1beta2ControllerRevision>(ListerFunc);
        }

        public Lister<V1ReplicaSet> V1ReplicaSet(IKubernetes operations)
        {
            async Task<IList<V1ReplicaSet>> ListerFunc(string @namespace)
            {
                var list = await operations.ListNamespacedReplicaSetAsync(@namespace);
                return list.Items;
            }
            return new Lister<V1ReplicaSet>(ListerFunc);
        }

        public Lister<V1StatefulSet> V1StatefulSet(IKubernetes operations)
        {
            async Task<IList<V1StatefulSet>> ListerFunc(string @namespace)
            {
                var list = await operations.ListNamespacedStatefulSetAsync(@namespace);
                return list.Items;
            }
            return new Lister<V1StatefulSet>(ListerFunc);
        }

        public Lister<V1beta1ControllerRevision> V1beta1ControllerRevision(IKubernetes operations)
        {
            async Task<IList<V1beta1ControllerRevision>> ListerFunc(string @namespace)
            {
                var list = await operations.ListNamespacedControllerRevision1Async(@namespace);
                return list.Items;
            }
            return new Lister<V1beta1ControllerRevision>(ListerFunc);
        }

        public Lister<V1ControllerRevision> V1ControllerRevision(IKubernetes operations)
        {
            async Task<IList<V1ControllerRevision>> ListerFunc(string @namespace)
            {
                var list = await operations.ListNamespacedControllerRevisionAsync(@namespace);
                return list.Items;
            }
            return new Lister<V1ControllerRevision>(ListerFunc);
        }

        public Lister<V1DaemonSet> V1DaemonSet(IKubernetes operations)
        {
            async Task<IList<V1DaemonSet>> ListerFunc(string @namespace)
            {
                var list = await operations.ListNamespacedDaemonSetAsync(@namespace);
                return list.Items;
            }
            return new Lister<V1DaemonSet>(ListerFunc);
        }

        public Lister<V1Deployment> V1Deployment(IKubernetes operations)
        {
            async Task<IList<V1Deployment>> ListerFunc(string @namespace)
            {
                var list = await operations.ListNamespacedDeploymentAsync(@namespace);
                return list.Items;
            }
            return new Lister<V1Deployment>(ListerFunc);
        }

        public Lister<V1ReplicationController> V1ReplicationController(IKubernetes operations)
        {
            async Task<IList<V1ReplicationController>> ListerFunc(string @namespace)
            {
                var list = await operations.ListNamespacedReplicationControllerAsync(@namespace);
                return list.Items;
            }
            return new Lister<V1ReplicationController>(ListerFunc);
        }

        public Lister<V1ResourceQuota> V1ResourceQuota(IKubernetes operations)
        {
            async Task<IList<V1ResourceQuota>> ListerFunc(string @namespace)
            {
                var list = await operations.ListNamespacedResourceQuotaAsync(@namespace);
                return list.Items;
            }
            return new Lister<V1ResourceQuota>(ListerFunc);
        }

        public Lister<V1Secret> V1Secret(IKubernetes operations)
        {
            async Task<IList<V1Secret>> ListerFunc(string @namespace)
            {
                var list = await operations.ListNamespacedSecretAsync(@namespace);
                return list.Items;
            }
            return new Lister<V1Secret>(ListerFunc);
        }

        public Lister<V1ServiceAccount> V1ServiceAccount(IKubernetes operations)
        {
            async Task<IList<V1ServiceAccount>> ListerFunc(string @namespace)
            {
                var list = await operations.ListNamespacedServiceAccountAsync(@namespace);
                return list.Items;
            }
            return new Lister<V1ServiceAccount>(ListerFunc);
        }

        public Lister<V1Service> V1Service(IKubernetes operations)
        {
            async Task<IList<V1Service>> ListerFunc(string @namespace)
            {
                var list = await operations.ListNamespacedServiceAsync(@namespace);
                return list.Items;
            }
            return new Lister<V1Service>(ListerFunc);
        }

        public Lister<V1Pod> V1Pod(IKubernetes operations)
        {
            async Task<IList<V1Pod>> ListerFunc(string @namespace)
            {
                var list = await operations.ListNamespacedPodAsync(@namespace);
                return list.Items;
            }
            return new Lister<V1Pod>(ListerFunc);
        }

        public Lister<V1PodTemplate> V1PodTemplate(IKubernetes operations)
        {
            async Task<IList<V1PodTemplate>> ListerFunc(string @namespace)
            {
                var list = await operations.ListNamespacedPodTemplateAsync(@namespace);
                return list.Items;
            }
            return new Lister<V1PodTemplate>(ListerFunc);
        }

        public Lister<V1ConfigMap> V1ConfigMap(IKubernetes operations)
        {
            async Task<IList<V1ConfigMap>> ListerFunc(string @namespace)
            {
                var list = await operations.ListNamespacedConfigMapAsync(@namespace);
                return list.Items;
            }
            return new Lister<V1ConfigMap>(ListerFunc);
        }

        public Lister<V1Endpoints> V1Endpoints(IKubernetes operations)
        {
            async Task<IList<V1Endpoints>> ListerFunc(string @namespace)
            {
                var list = await operations.ListNamespacedEndpointsAsync(@namespace);
                return list.Items;
            }
            return new Lister<V1Endpoints>(ListerFunc);
        }

        public Lister<V1Event> V1Event(IKubernetes operations)
        {
            async Task<IList<V1Event>> ListerFunc(string @namespace)
            {
                var list = await operations.ListNamespacedEventAsync(@namespace);
                return list.Items;
            }
            return new Lister<V1Event>(ListerFunc);
        }

        public Lister<V1LimitRange> V1LimitRange(IKubernetes operations)
        {
            async Task<IList<V1LimitRange>> ListerFunc(string @namespace)
            {
                var list = await operations.ListNamespacedLimitRangeAsync(@namespace);
                return list.Items;
            }
            return new Lister<V1LimitRange>(ListerFunc);
        }

        public Lister<V1PersistentVolumeClaim> V1PersistentVolumeClaim(IKubernetes operations)
        {
            async Task<IList<V1PersistentVolumeClaim>> ListerFunc(string @namespace)
            {
                var list = await operations.ListNamespacedPersistentVolumeClaimAsync(@namespace);
                return list.Items;
            }
            return new Lister<V1PersistentVolumeClaim>(ListerFunc);
        }
    }
}
