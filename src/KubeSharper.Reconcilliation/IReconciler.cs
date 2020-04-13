using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KubeSharper.Reconcilliation
{
    public interface IReconciler
    {
        Task<ReconcileResult> Reconcile(ReconcileContext context, ReconcileRequest request);
    }

    public delegate Task<ReconcileResult> ReconcileFunc(
        ReconcileContext context, ReconcileRequest request);

    class ReconcileWrapper : IReconciler
    {
        private readonly ReconcileFunc _reconciler;
        public ReconcileWrapper(ReconcileFunc reconciler)
        {
            _reconciler = reconciler;
        }

        public async Task<ReconcileResult> Reconcile(
            ReconcileContext context, ReconcileRequest request)
        {
            return await _reconciler(context, request);
        }
    }

    public static class ReconcileFuncExt
    {
        public static IReconciler ToReconciler(this ReconcileFunc f) => new ReconcileWrapper(f);
    }
}
