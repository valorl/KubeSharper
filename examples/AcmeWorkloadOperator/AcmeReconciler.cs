using KubeSharper.Reconcilliation;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AcmeWorkloadOperator
{
    public class AcmeReconciler : IReconciler
    {
        public async Task<ReconcileResult> Reconcile(ReconcileContext context, ReconcileRequest request)
        {
            Log.Information($"[{nameof(AcmeReconciler)}] Request received for {request}");
            return await Task.FromResult(new ReconcileResult());
        }
    }
}
