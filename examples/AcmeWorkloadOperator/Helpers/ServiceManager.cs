using k8s;
using k8s.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Rest;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AcmeWorkloadOperator.Helpers
{
    public class ServiceManager : IManagerFor<AcmeService>
    {
        private readonly IKubernetes _client;
        public ServiceManager(IKubernetes client)
        {
            _client = client;
        }

        public async Task Apply(AcmeService owner, string name, string @namespace)
        {
            var obj = Make(owner, name, @namespace);
            try
            {
                HttpOperationResponse<V1Service> existing;
                try
                {
                    existing = await _client.ReadNamespacedServiceWithHttpMessagesAsync(
                        obj.Metadata.Name, obj.Metadata.NamespaceProperty);
                }
                catch(HttpOperationException ex) when (ex.Response.StatusCode == HttpStatusCode.NotFound)
                {
                    await _client.CreateNamespacedServiceAsync(obj, obj.Metadata.NamespaceProperty);
                }
                catch(HttpOperationException ex)
                {
                    Log.Error($"Unexpected status code ({ex.Response.StatusCode}) when fetching existing service.");
                    throw;
                }

                var patch = new JsonPatchDocument<V1Service>()
                    .Replace(o => o.Metadata.Labels, obj.Metadata.Labels)
                    .Replace(o => o.Spec.Ports, obj.Spec.Ports);
                await _client.PatchNamespacedServiceAsync(
                    new V1Patch(patch), obj.Metadata.Name, obj.Metadata.NamespaceProperty);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error applying service {obj.Metadata.NamespaceProperty}/{obj.Metadata.Name} ");
                throw;
            }
        }
        private V1Service Make(AcmeService owner, string name, string @namespace)
        {
            var spec = owner.Spec;
            var service = new V1Service
            {
                Metadata = new V1ObjectMeta
                {
                    NamespaceProperty = @namespace,
                    Name = name,
                    OwnerReferences = OwnerReferenceFactory.NewList(owner),
                    Labels = spec.Labels
                        .WithKeyValue("acme-team", spec.Team)
                },
                Spec = new V1ServiceSpec
                {
                    Selector = new Dictionary<string, string>()
                    {
                        {"app", name}
                    },
                    Ports = new List<V1ServicePort>
                    {
                        new V1ServicePort
                        {
                            Protocol = "TCP",
                            Name = "http",
                            Port = 80,
                            TargetPort = spec.Port
                        }
                    },
                }
            };
            return service;
        }
    }
}
