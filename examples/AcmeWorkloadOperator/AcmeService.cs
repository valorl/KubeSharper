using k8s.Models;
using KubeSharper.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcmeWorkloadOperator
{
    [CustomResourceDefinition("acme.dev", "v1", "acmeapplications", "acmeapplication")]
    public class AcmeService : CustomResource<AcmeServiceSpec, AcmeServiceStatus>
    {
    }

    public class AcmeServiceSpec
    {
        public string Name { get; set; }
        public int Replicas { get; set; }
        public int ImageName { get; set; }
        public int ImageVersion { get; set; }
        public bool Headless { get; set; }
        public int TargetPort { get; set; }
        public V1ResourceRequirements Resources { get; set; }
        public V1SecretReference CredentialsSecret { get; set; }
        public Dictionary<string, string> Labels { get; set; }
        public Dictionary<string, string> Config { get; set; }
        public Dictionary<string, string> Environment { get; set; }
    }

    public class AcmeServiceStatus
    {
        public List<string> Secrets { get; set; }
        public string ConfigMap { get; set; }
        public string Deployment { get; set; }
        public string Service { get; set; }
        public string Ingress { get; set; }
    }
}
