using k8s;
using k8s.Models;
using KubeSharper.Utils;
using Microsoft.Rest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AcmeWorkloadOperator.E2ETests
{
    public class TestBase
    {
        private const string KUBECONFIG = @"C:\Users\vao\kubeconfig.yaml";

        protected IKubernetes Kubernetes { get; }
        protected string Namespace { get; private set; }
        public TestBase()
        {
            var config = KubernetesClientConfiguration.BuildConfigFromConfigFile(KUBECONFIG);
            Kubernetes = new Kubernetes(config);
        }


        public async Task NewNamespace()
        {
            var ns = new V1Namespace
            {
                ApiVersion = "v1",
                Kind = "Namespace",
                Metadata = new V1ObjectMeta
                {
                    GenerateName = "acme-e2e-"
                }
            };
            var created = await Kubernetes.CreateNamespaceAsync(ns).ConfigureAwait(false);
            Namespace = created.Metadata.Name;
        }


        public async Task KubectlApply(string path)
        {
            using var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "kubectl",
                    Arguments = $"apply -f {path}",
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                }
            };
            proc.StartInfo.EnvironmentVariables.Add("KUBECONFIG", KUBECONFIG);

            proc.Start();
            proc.WaitForExit();
            if(proc.ExitCode != 0)
            {
                var stdOut = await proc.StandardOutput.ReadToEndAsync().ConfigureAwait(false);
                var stdErr = proc.StandardError.ReadToEndAsync().ConfigureAwait(false);

                throw new Exception($"STDOUT: \n {stdOut} \n --- \n {stdErr}");
            }
        }

        public async Task CleanupNamespace()
        {
            await Kubernetes.DeleteNamespaceAsync(Namespace).ConfigureAwait(false);
        }

        public async Task CreateCustomObject<T>(T obj)
        {
            var crd = CustomResourceDefinition.For<T>();
            var jObj = JObject.FromObject(obj, new JsonSerializer
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy
                    {
                        ProcessDictionaryKeys = false,
                    }
                }
            });
            await Kubernetes.CreateNamespacedCustomObjectAsync(jObj, crd.Group, crd.Version, Namespace, crd.Plural)
                .ConfigureAwait(false);
        }
        public async Task<T> GetCustomObject<T>(string name)
        {
            var crd = CustomResourceDefinition.For<T>();
            var obj = await Kubernetes.GetNamespacedCustomObjectAsync(crd.Group, crd.Version, Namespace, crd.Plural, name)
                .ConfigureAwait(false);
            var jObj = (JObject)obj;
            return jObj.ToObject<T>();
        }

        public async Task<T> Get<T>(string name, CancellationToken ct = default)
        {
            try
            {
                if (typeof(T) == typeof(V1Deployment))
                    return (T)(object)(await Kubernetes.ReadNamespacedDeploymentAsync(name, Namespace, cancellationToken: ct).ConfigureAwait(false));
                if (typeof(T) == typeof(V1Service))
                    return (T)(object)(await Kubernetes.ReadNamespacedServiceAsync(name, Namespace, cancellationToken: ct).ConfigureAwait(false));
            }
            catch (HttpOperationException ex) when (ex.Response.StatusCode == HttpStatusCode.NotFound) { }
            return default(T);
        }

        public async Task<T> Poll<T>(string name, TimeSpan timeout = default)
        {
            if (timeout == default) timeout = TimeSpan.FromSeconds(30);
            var cts = new CancellationTokenSource(timeout);

            T obj = default;
            while(!cts.IsCancellationRequested && obj == null)
            {
                obj = await Get<T>(name, cts.Token).ConfigureAwait(false);
                await Task.Delay(1000, cts.Token).ConfigureAwait(false);
            }
            return obj;
        }
    }
}
