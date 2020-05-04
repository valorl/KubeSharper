using k8s.Models;
using KubeSharper.WorkQueue;
using KubeSharper.Reconcilliation;
using KubeSharper.Utils;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KubeSharper
{
    public static class Handlers
    {
        public delegate Task EnqueueingHandler(EventType et, KubernetesV1MetaObject obj, IWorkQueue<ReconcileRequest> queue);
        public static EnqueueingHandler EnqueueForObject()
        {
            return LogException(async (et, obj, q) =>
            {
                var r = MakeRequest(obj);
                await EnqueueRequest(q, r);
            });
        }

        public static EnqueueingHandler EnqueueForOwner(bool isController)
        {
            return LogException(async (et, obj, q) =>
            {
                var ownerReferences = obj.Metadata.OwnerReferences;
                if (ownerReferences == null || ownerReferences.Count == 0)
                {
                    Log.Debug($"Skipping no owner reference: {obj.ApiVersion}/{obj.Metadata.NamespaceProperty}/{obj.Kind}/{obj.Metadata.Name}");
                    return;
                }

                var requests = isController switch
                {
                    true =>
                        ownerReferences
                        .Where(r => r.Controller.HasValue && r.Controller == true)
                        .Select(r => MakeRequest(obj, r)),

                    false =>
                        ownerReferences.Select(r => MakeRequest(obj, r))
                };
                foreach (var r in requests)
                {
                    await EnqueueRequest(q, r);
                }
            });
        }

        private static ReconcileRequest MakeRequest(
            KubernetesV1MetaObject obj,
            V1OwnerReference owner = null) => (owner == null) switch
        {
            true => new ReconcileRequest
            {
                ApiVersion = obj.ApiVersion,
                Kind = obj.Kind,
                Namespace = obj.Metadata.NamespaceProperty,
                Name = obj.Metadata.Name
            },

            false => new ReconcileRequest
            {
                ApiVersion = owner.ApiVersion,
                Kind = owner.Kind,
                Namespace = obj.Metadata.NamespaceProperty,
                Name = obj.Metadata.Name
            }
        };

        private static async Task EnqueueRequest(IWorkQueue<ReconcileRequest> queue, ReconcileRequest req)
        {
            if (!await queue.TryAdd(req))
            {
                Log.Warning($"Request not added ({req})");
            }
            else
            {
                Log.Information($"Added request for {req}");
            }
        }

        private static EnqueueingHandler LogException(EnqueueingHandler h)
        {
            return async (et, obj, q) =>
            {
                try
                {
                    await h(et, obj, q);
                }
                catch(Exception ex)
                {
                    Log.Error(ex,
                        $"Handler failed on {et} of {obj.ApiVersion}/{obj.Kind}/{obj.Metadata.NamespaceProperty}/{obj.Metadata.Name}");
                    throw;
                }
            };
        }

    }

    public class OwnerInfo
    {
        public ApiVersionKind ApiVersion { get; set; }
        public bool IsController { get; set; }
    }
}
