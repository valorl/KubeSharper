using k8s;
using k8s.Models;
using KubeSharper.EventQueue;
using KubeSharper.EventSources;
using KubeSharper.Utils;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubeSharper.Reconcilliation
{
    public static class Handlers
    {
        public static EventSourceHandler ObjectEnqueuer(IEventQueue<ReconcileRequest> queue)
        {
            return LogException(async (et, obj) =>
            {
                var r = MakeRequest(obj);
                await EnqueueRequest(queue, r);
            });
        }

        public static EventSourceHandler OwnerEnqueuer(IEventQueue<ReconcileRequest> queue, OwnerInfo info)
        {
            return LogException(async (et, obj) =>
            {
                var requests = info.IsController switch
                {
                    true =>
                        obj.Metadata.OwnerReferences
                        .Where(r => r.Controller.HasValue && r.Controller == true)
                        .Select(r => MakeRequest(obj, r)),

                    false =>
                        obj.Metadata.OwnerReferences
                        .Select(r => MakeRequest(obj, r))
                };
                foreach (var r in requests)
                {
                    await EnqueueRequest(queue, r);
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

        private static async Task EnqueueRequest(IEventQueue<ReconcileRequest> queue, ReconcileRequest req)
        {
            if (!(await queue.TryAdd(req)))
            {
                Log.Error($"Failed adding {req.ApiVersion}/{req.Namespace}/{req.Kind}/{req.Name}");
            }
            else
            {
                Log.Information($"Added {req}");
            }
        }

        private static EventSourceHandler LogException(EventSourceHandler h)
        {
            return async (et, obj) =>
            {
                try
                {
                    await h(et, obj);
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
