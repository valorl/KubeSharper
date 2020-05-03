using Google.Api.Gax.ResourceNames;
using Google.Cloud.Redis.V1;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
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
            var instanceName = new InstanceName(ProjectId, LocationId, obj.Status.Id);
            var existing = await _client.GetInstanceAsync(instanceName);
            if(existing == null)
            {
                var instance = obj.ToInstance(ProjectId, LocationId);
                var parent = LocationName.FormatProjectLocation(
                    _settings.GoogleProjectId, _settings.GoogleRegion);
                var op = await _client.CreateInstanceAsync(parent, obj.Spec.Name, instance);
                await op.PollUntilCompletedAsync();
                return op.Result;
            }

            var changedFields = obj.GetChangedInstanceFields(existing);
            if(changedFields.Count > 0)
            {
                var mask = new FieldMask();
                mask.Paths.AddRange(changedFields);
                var instance = obj.ToInstance(ProjectId, LocationId);

                var op = await _client.UpdateInstanceAsync(mask, instance);
                await op.PollUntilCompletedAsync();
                return op.Result;
            }
            return existing;
        }

    }
}
