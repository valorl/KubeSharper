using AcmeWorkloadOperator.Helpers;
using k8s;
using k8s.Models;
using KubeSharper.Reconcilliation;
using KubeSharper.Utils;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Rest;
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
                Log.Error($"Request object not found: {req}.");
                //TODO: Garbage collection ?
            }
            var obj = ((JObject)response.Body).ToObject<AcmeService>();
            var spec = obj.Spec;

            var (name, @namespace) = (obj.Metadata.Name, obj.Metadata.NamespaceProperty);

            var deploymentApplier = new DeploymentApplier(client);
            await deploymentApplier.Apply(obj, name, @namespace);

            var serviceApplier = new ServiceApplier(client);
            await serviceApplier.Apply(obj, name, @namespace);

            return new ReconcileResult();
        }


    }
}
