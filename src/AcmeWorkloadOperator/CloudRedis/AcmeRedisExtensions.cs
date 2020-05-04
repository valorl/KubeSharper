using Google.Cloud.Redis.V1;
using System;
using System.Collections.Generic;
using System.Text;
using static Google.Cloud.Redis.V1.Instance.Types;

namespace AcmeWorkloadOperator.CloudRedis
{
    public static class AcmeRedisExtensions
    {
        public static Instance ToInstance(this AcmeRedis obj, string projectId, string locationId)
        {
            var instance = new Instance
            {
                Name = InstanceName.FormatProjectLocationInstance(
                    locationId, locationId, obj.Spec.Name),
                MemorySizeGb = obj.Spec.MemorySizeGb,
                Tier = Tier.Basic
            };
            instance.Labels.Add("created-by", "acme-cloud-redis-reconciler");
            return instance;
        }

        public static List<string> GetChangedInstanceFields(this AcmeRedis obj, Instance instance)
        {
            var fields = new List<string>();
            if (instance.MemorySizeGb != obj.Spec.MemorySizeGb)
                fields.Add("memorySizeGb");
            return fields;
        }
    }
}
