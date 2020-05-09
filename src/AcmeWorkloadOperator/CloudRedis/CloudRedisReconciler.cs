using AcmeWorkloadOperator.Utils;
using Google.Cloud.Redis.V1;
using k8s;
using k8s.Models;
using KubeSharper.Reconcilliation;
using KubeSharper.Utils;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AcmeWorkloadOperator.CloudRedis
{
    public class CloudRedisReconciler : IReconciler
    {
        private const string FINALIZER = "finalizer.acme.dev";

        private readonly RedisInstanceManager _cloudRedis;
        public CloudRedisReconciler(RedisInstanceManager manager)
        {
            _cloudRedis = manager;
        }
        public async Task<ReconcileResult> Reconcile(ReconcileContext context, ReconcileRequest request)
        {
            Log.Information($"[{this.GetType().Name}] Request received for {request}");
            var client = context.Client;

            var crd = CustomResourceDefinition.For<AcmeRedis>();
            var response = await client.GetNamespacedCustomObjectWithHttpMessagesAsync(
                crd.Group, crd.Version, request.Namespace, crd.Plural, request.Name);
            if(response.Response.StatusCode == HttpStatusCode.NotFound)
            {
                // This will likely happen after a post-finalizer delete
                Log.Warning($"Request object not found: {request}.");
            }
            var obj = ((JObject)response.Body).ToObject<AcmeRedis>();

            var finalizers = obj.Metadata.Finalizers;
            if(!finalizers?.Contains(FINALIZER) ?? true)
            {
                await PatchAddFinalizer(client, obj);
            }
            else if(obj.Metadata.DeletionTimestamp.HasValue)
            {
                var deleted = await _cloudRedis.Delete(obj);
                if(!deleted)
                {
                    Log.Error($"Redis instance delete failed for {obj.Metadata.Name}");
                }

                await PatchRemoveFinalizer(client, obj);
                await DeleteAcmeRedis(client, obj);

                return new ReconcileResult();
            }

            // Culprit? Shouldn't try to create if already exists...
            var instance = await _cloudRedis.CreateOrUpdate(obj);
            _ = await PatchStatus(client, obj, instance);

            return new ReconcileResult();
        }

        private async Task<AcmeRedis> PatchStatus(IKubernetes client, AcmeRedis obj, Instance instance)
        {
            var statusNull = obj.Status == null;
            obj.Status = new AcmeRedisStatus
            {
                Id = instance.InstanceName.InstanceId,
                State = instance.State.ToString(),
                Location = instance.InstanceName.LocationId,
                CreatedAt = instance.CreateTime.ToDateTime(),
                Host = instance.Host,
                Port = instance.Port
            };

            var patch = Patch.New<AcmeRedis>();
            if (statusNull)
            {
                patch
                    .Add(o => o.Status, new AcmeRedisStatus())
                    .Add(o => o.Status.Id, obj.Status.Id)
                    .Add(o => o.Status.State, obj.Status.State)
                    .Add(o => o.Status.Location, obj.Status.Location)
                    .Add(o => o.Status.CreatedAt, obj.Status.CreatedAt)
                    .Add(o => o.Status.Host, obj.Status.Host)
                    .Add(o => o.Status.Port, obj.Status.Port);
            }
            else
            {
                patch
                    .Replace(o => o.Status.Id, obj.Status.Id)
                    .Replace(o => o.Status.State, obj.Status.State)
                    .Replace(o => o.Status.Location, obj.Status.Location)
                    .Replace(o => o.Status.CreatedAt, obj.Status.CreatedAt)
                    .Replace(o => o.Status.Host, obj.Status.Host)
                    .Replace(o => o.Status.Port, obj.Status.Port);
            }

            return await PatchAcmeRedis(client, obj, patch);
        }

        private async Task<AcmeRedis> PatchAddFinalizer(IKubernetes client, AcmeRedis obj)
        {
            var finalizers = obj.Metadata.Finalizers ?? new List<string>();

            var patch = Patch.New<AcmeRedis>();
            if (finalizers.Count == 0)
            {
                finalizers.Add(FINALIZER);
                patch.Add(o => o.Metadata.Finalizers, finalizers);
            }
            else
            {
                finalizers.Add(FINALIZER);
                patch.Replace(o => o.Metadata.Finalizers, finalizers);
            }

            return await PatchAcmeRedis(client, obj, patch);
        }

        private async Task<AcmeRedis> PatchRemoveFinalizer(IKubernetes client, AcmeRedis obj)
        {
            var finalizers = obj.Metadata.Finalizers;
            if (finalizers == null || finalizers.Count == 0)
                throw new ArgumentException("Object has no finalizers");

            var patch = Patch.New<AcmeRedis>();
            if (finalizers.Count == 1) // Only FINALIZER
            {
                patch.Remove(o => o.Metadata.Finalizers);
            }
            else if (finalizers.Count > 1)
            {
                finalizers.Remove(FINALIZER);
                patch.Replace(o => o.Metadata.Finalizers, finalizers);
            }

            return await PatchAcmeRedis(client, obj, patch);
        }

        private async Task<AcmeRedis> PatchAcmeRedis(IKubernetes client, AcmeRedis obj, JsonPatchDocument<AcmeRedis> patch)
        {

            var @namespace = obj.Metadata.NamespaceProperty;
            var name = obj.Metadata.Name;
            var crd = CustomResourceDefinition.For<AcmeRedis>();
            var response =  await client.PatchNamespacedCustomObjectWithHttpMessagesAsync(
                new V1Patch(patch), crd.Group, crd.Version, @namespace, crd.Plural, name);
            var jObj = (JObject)response.Body;
            var patched = jObj.ToObject<AcmeRedis>();
            return patched;
        }

        private async Task DeleteAcmeRedis(IKubernetes client, AcmeRedis obj)
        {
            var @namespace = obj.Metadata.NamespaceProperty;
            var name = obj.Metadata.Name;
            var crd = CustomResourceDefinition.For<AcmeRedis>();
            _ = await client.DeleteNamespacedCustomObjectWithHttpMessagesAsync(
                crd.Group, crd.Version, @namespace, crd.Plural, name);
        }
    }
}
