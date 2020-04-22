﻿using k8s;
using k8s.Models;
using KubeSharper.Reconcilliation;
using KubeSharper.Utils;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AcmeWorkloadOperator.Reconciliation
{
    public class AcmeReconciler : IReconciler
    {
        public async Task<ReconcileResult> Reconcile(ReconcileContext context, ReconcileRequest request)
        {
            Log.Information($"[{nameof(AcmeReconciler)}] Request received for {request}");
            var client = context.Client;
            var result = await DoAcmeService(client, request);
            return result;
        }

        private async Task<ReconcileResult> DoAcmeService(IKubernetes client, ReconcileRequest req)
        {
            var crd = CustomResourceDefinition.For<AcmeService>();
            var response = await client.GetNamespacedCustomObjectWithHttpMessagesAsync(
                crd.Group, crd.Version, req.Namespace, crd.Plural, req.Name);
            if(response.Response.StatusCode == HttpStatusCode.NotFound)
            {
                Log.Error($"Request object not found: {req}");
            }
            var obj = ((JObject)response.Body).ToObject<AcmeService>();
            var spec = obj.Spec;
            var newDeployment = MakeDeployment(spec, obj.Metadata.NamespaceProperty);

            await ApplyDeployment(client, newDeployment);
            return new ReconcileResult();
        }


        private V1Deployment MakeDeployment(AcmeServiceSpec spec, string @namespace)
        {
            var deployment = new V1Deployment
            {
                Metadata = new V1ObjectMeta
                {
                    NamespaceProperty = @namespace,
                    Name = spec.Name,
                    Labels = spec.Labels
                        .WithKeyValue("acme-team", spec.Team)
                },
                Spec = new V1DeploymentSpec
                {
                    Replicas = spec.Replicas,
                    Template = new V1PodTemplateSpec
                    {
                        Metadata = new V1ObjectMeta
                        {
                            Labels = new Dictionary<string, string> { {"app", spec.Name} }
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
                                            HostPort = 80,
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

        private async Task ApplyDeployment(IKubernetes client, V1Deployment dep)
        {
            var existing = await client.ReadNamespacedDeploymentAsync(
                dep.Metadata.Name, dep.Metadata.NamespaceProperty);
            if(existing == null)
            {
                await client.CreateNamespacedDeploymentAsync(dep, dep.Metadata.NamespaceProperty);
            }
            else
            {
                var patch = new JsonPatchDocument<V1Deployment>()
                    .Replace(o => o.Metadata.Labels, dep.Metadata.Labels)
                    .Replace(o => o.Spec.Replicas, dep.Spec.Replicas)
                    .Replace(o => o.Spec.Template.Metadata.Labels, dep.Spec.Template.Metadata.Labels)
                    .Replace(o => o.Spec.Template.Spec.Containers, dep.Spec.Template.Spec.Containers);
                await client.PatchNamespacedDeploymentAsync(
                    new V1Patch(patch), dep.Metadata.Name, dep.Metadata.NamespaceProperty);
            }
        }



    }
}