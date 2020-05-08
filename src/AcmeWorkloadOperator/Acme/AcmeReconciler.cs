﻿using AcmeWorkloadOperator.Utils;
using k8s;
using k8s.Models;
using KubeSharper.Reconcilliation;
using KubeSharper.Utils;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Rest;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AcmeWorkloadOperator.Acme
{
    public class AcmeReconciler : IReconciler
    {
        //private readonly DeploymentManager _deploymentMgr;
        //private readonly ServiceManager _serviceMgr;

        //public AcmeReconciler(DeploymentManager deploymentMgr, ServiceManager serviceMgr)
        //{
        //    _deploymentMgr = deploymentMgr;
        //    _serviceMgr = serviceMgr;
        //}
        public async Task<ReconcileResult> Reconcile(ReconcileContext context, ReconcileRequest request)
        {
            Log.Information($"[{this.GetType().Name}] Request received for {request}");
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
                Log.Error($"Request object not found: {req}.");
                return new ReconcileResult();
                // Garbage collection happens automatically since all resources are native
            }
            var obj = ((JObject)response.Body).ToObject<AcmeService>();

            var (name, @namespace) = (obj.Metadata.Name, obj.Metadata.NamespaceProperty);

            var deploymentApplier = new DeploymentManager(client);
            var deployment = await deploymentApplier.Apply(obj, name, @namespace);

            var serviceApplier = new ServiceManager(client);
            var service = await serviceApplier.Apply(obj, name, @namespace);

            await PatchServiceInfo(client, obj, service);
            return new ReconcileResult();
        }

        private async Task<AcmeService> PatchServiceInfo(IKubernetes client, AcmeService owner, V1Service service)
        {


            var crd = CustomResourceDefinition.For<AcmeService>();
            var @namespace = owner.Metadata.NamespaceProperty;
            var name = owner.Metadata.Name;

            var patch = (owner.Status) switch
            {
                null => Patch.New<AcmeService>()
                    .Add(o => o.Status, new AcmeServiceStatus())
                    .Add(o => o.Status.ServiceIP, service.Spec.ClusterIP),

                _ => Patch.New<AcmeService>().Replace(o => o.Status.ServiceIP, service.Spec.ClusterIP)
            };
            return await PatchAcmeService(client, owner, patch);
        }


        private async Task<AcmeService> PatchAcmeService(
            IKubernetes client, AcmeService obj, JsonPatchDocument<AcmeService> patch)
        {
            var @namespace = obj.Metadata.NamespaceProperty;
            var name = obj.Metadata.Name;
            var crd = CustomResourceDefinition.For<AcmeService>();
            var response =  await client.PatchNamespacedCustomObjectWithHttpMessagesAsync(
                new V1Patch(patch), crd.Group, crd.Version, @namespace, crd.Plural, name);
            var jObj = (JObject)response.Body;
            var patched = jObj.ToObject<AcmeService>();
            return patched;
        }
    }
}