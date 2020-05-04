using Google.Api.Gax;
using Google.Api.Gax.Grpc;
using Google.Api.Gax.ResourceNames;
using Google.Cloud.Redis.V1;
using Google.LongRunning;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AcmeWorkloadOperator.CloudRedis
{
    public class RedisInstanceManagerSettings
    {
        public string GoogleProjectId { get; set; }
        public string GoogleRegion { get; set; }
    }

    public class RedisInstanceManager
    {
        private readonly CloudRedisClient _client;
        private readonly RedisInstanceManagerSettings _settings;

        private string ProjectId => _settings.GoogleProjectId;
        private string LocationId => _settings.GoogleRegion;
        public RedisInstanceManager(
            CloudRedisClient client,
            RedisInstanceManagerSettings settings)
        {
            _client = client;
            _settings = settings;
        }

        public async Task<Instance> CreateOrUpdate(AcmeRedis obj)
        {
            var instanceName = new InstanceName(ProjectId, LocationId, obj.Spec.Name);
            Instance existing = null;
            try { await _client.GetInstanceAsync(instanceName); }
            catch (RpcException e) when (e.StatusCode == StatusCode.NotFound) { }

            if(existing == null)
            {
                var instance = obj.ToInstance(ProjectId, LocationId);
                var parent = LocationName.FormatProjectLocation(
                    _settings.GoogleProjectId, _settings.GoogleRegion);
                var op = await _client.CreateInstanceAsync(parent, obj.Spec.Name, instance);
                var completed = await UntilCompleted(op);
                return completed.Result;
            }

            var changedFields = obj.GetChangedInstanceFields(existing);
            if(changedFields.Count > 0)
            {
                var mask = new FieldMask();
                mask.Paths.AddRange(changedFields);
                var instance = obj.ToInstance(ProjectId, LocationId);

                var op = await _client.UpdateInstanceAsync(mask, instance);
                var completed = await UntilCompleted(op);
                return completed.Result;
            }
            return existing;
        }

        public async Task<bool> Delete(AcmeRedis obj)
        {
            var instanceName = new InstanceName(ProjectId, LocationId, obj.Spec.Name);
            var op = await _client.DeleteInstanceAsync(instanceName);
            var completed = await UntilCompleted(op);
            return !completed.IsFaulted;
        }

        private async Task<Operation<T,U>> UntilCompleted<T,U>(Operation<T,U> operation)
            where T : class, IMessage<T>, new()
            where U : class, IMessage<U>, new()
        {
            return await operation.PollUntilCompletedAsync(new PollSettings(
                expiration: Expiration.FromTimeout(TimeSpan.FromMinutes(30)),
                delay: TimeSpan.FromSeconds(10)));
        }

    }
}
