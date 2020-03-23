using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KubeSharper.Reconcilliation
{
    public interface IReconciler
    {
        Task<ReconcileResult> Reconcile(ReconcileRequest request);
    }

    public delegate Task<ReconcileResult> ReconcileFunc(ReconcileRequest request);
}
