using KubeSharper.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcmeWorkloadOperator.CloudRedis
{
    public class AcmeRedis : CustomResource<RedisInstanceSpec, RedisInstanceStatus>
    {
    }

    public class RedisInstanceSpec
    {
        public string Name { get; set; }
        public int MemorySizeGb { get; set; }
    }

    public class RedisInstanceStatus
    {
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Location { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }
}
