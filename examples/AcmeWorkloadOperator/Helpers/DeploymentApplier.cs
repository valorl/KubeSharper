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
    public class DeploymentApplier : IForOwnerApplier<AcmeService>
    {
        private readonly IKubernetes _client;
        public DeploymentApplier(IKubernetes client)
        {
            _client = client;
        }

        public async Task Apply(AcmeService owner, string name, string @namespace)
        {
            var obj = Make(owner, name, @namespace);
            try
            {
                HttpOperationResponse<V1Deployment> existing;
                try
                {
                    existing = await _client.ReadNamespacedDeploymentWithHttpMessagesAsync(
                        obj.Metadata.Name, obj.Metadata.NamespaceProperty);
                }
                catch(HttpOperationException ex) when (ex.Response.StatusCode == HttpStatusCode.NotFound)
                {
                    await _client.CreateNamespacedDeploymentAsync(obj, obj.Metadata.NamespaceProperty);
                }
                catch(HttpOperationException ex)
                {
                    Log.Error($"Unexpected status code ({ex.Response.StatusCode}) when fetching existing deployment.");
                    throw;
                }

                var patch = new JsonPatchDocument<V1Deployment>()
                    .Replace(o => o.Metadata.Labels, obj.Metadata.Labels)
                    .Replace(o => o.Spec.Replicas, obj.Spec.Replicas)
                    .Replace(o => o.Spec.Template.Metadata.Labels, obj.Spec.Template.Metadata.Labels)
                    .Replace(o => o.Spec.Template.Spec.Containers, obj.Spec.Template.Spec.Containers);
                await _client.PatchNamespacedDeploymentAsync(
                    new V1Patch(patch), obj.Metadata.Name, obj.Metadata.NamespaceProperty);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error applying deployment {obj.Metadata.NamespaceProperty}/{obj.Metadata.Name} ");
                throw;
            }
        }
        private V1Deployment Make(AcmeService owner, string name, string @namespace)
        {
            var spec = owner.Spec;
            var deployment = new V1Deployment
            {
                Metadata = new V1ObjectMeta
                {
                    NamespaceProperty = @namespace,
                    Name = name,
                    OwnerReferences = new List<V1OwnerReference>
                    {
                        new V1OwnerReference
                        {
                            Controller = true,
                            ApiVersion = "acme.dev/v1",
                            Kind = "AcmeService",
                            Name = owner.Metadata.Name,
                            Uid = owner.Metadata.Uid
                        }
                    },
                    Labels = spec.Labels
                        .WithKeyValue("acme-team", spec.Team)
                },
                Spec = new V1DeploymentSpec
                {
                    Replicas = spec.Replicas,
                    Selector = new V1LabelSelector
                    {
                        MatchLabels = new Dictionary<string,string>()
                        {
                            {"app", name} 
                        }
                    },
                    Template = new V1PodTemplateSpec
                    {
                        Metadata = new V1ObjectMeta
                        {
                            Labels = new Dictionary<string, string> { {"app", name} }
                                .WithKeyValue("acme-team", spec.Team),
                            
                        },
                        Spec = new V1PodSpec
                        {
                            Containers = new List<V1Container>
                            {
                                new V1Container
                                {
                                    Name = "app",
                                    Image = $"{spec.ImageName}:{spec.ImageVersion}",
                                    Ports =  new List<V1ContainerPort>
                                    {
                                        new V1ContainerPort
                                        {
                                            ContainerPort = spec.Port,
                                        }
                                    }
                                    
                                }
                            }
                        }
                    }
                }
            };
            return deployment;
        }

    }
}
