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

    class ReconcileWrapper : IReconciler
    {
        private readonly ReconcileFunc _reconciler;
        public ReconcileWrapper(ReconcileFunc reconciler)
        {
            _reconciler = reconciler;
        }

        public async Task<ReconcileResult> Reconcile(ReconcileRequest request)
        {
            return await _reconciler(request);
        }
    }

    public static class ReconcileFuncExt
    {
        public static IReconciler ToReconciler(this ReconcileFunc f) => new ReconcileWrapper(f);

    }
}
