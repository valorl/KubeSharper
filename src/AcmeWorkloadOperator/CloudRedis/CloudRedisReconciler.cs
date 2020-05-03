using KubeSharper.Reconcilliation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AcmeWorkloadOperator.CloudRedis
{
    public class CloudRedisReconciler : IReconciler
    {
        public Task<ReconcileResult> Reconcile(ReconcileContext context, ReconcileRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
