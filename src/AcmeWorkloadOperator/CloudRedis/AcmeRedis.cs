using KubeSharper.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using static Google.Cloud.Redis.V1.Instance.Types;

namespace AcmeWorkloadOperator.CloudRedis
{
    [CustomResourceDefinition("acme.dev", "v1", "acmeredises", "acmeredis")]
    public class AcmeRedis : CustomResource<AcmeRedisSpec, AcmeRedisStatus>
    {
    }

    public class AcmeRedisSpec
    {
        public string Name { get; set; }
        public int MemorySizeGb { get; set; }
        public Tier Tier { get; set; }
    }

    public class AcmeRedisStatus
    {
        public string Id { get; set; }
        public string State { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Location { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }
}
