using k8s.Models;
using KubeSharper.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcmeWorkloadOperator
{
    [CustomResourceDefinition("acme.dev", "v1", "acmeservices", "acmeservice")]
    public class AcmeService : CustomResource<AcmeServiceSpec, AcmeServiceStatus>
    {
    }

    public class AcmeServiceSpec
    {
        public AcmeServiceSpec()
        {
            Labels = new Dictionary<string, string>();
            Config = new Dictionary<string, string>();
            Environment = new Dictionary<string, string>();
        }
        public string Name { get; set; }
        public string Team { get; set; }
        public int Replicas { get; set; }
        public string ImageName { get; set; }
        public string ImageVersion { get; set; }
        public bool Headless { get; set; }
        public int Port { get; set; }
        public AcmeServiceResources Resources { get; set; }
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

    public class AcmeServiceResources
    {
        public Dictionary<string,string> Limits { get; set; }
        public Dictionary<string,string> Requests { get; set; }
    }
}

