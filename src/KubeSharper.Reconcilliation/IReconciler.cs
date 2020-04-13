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

    public static class Reconciler
    {
        public static IReconciler Wrap(
            Func<ReconcileContext,ReconcileRequest,Task<ReconcileResult>> func)
        {
            return new ReconcileWrapper(func);
        }
    }

    class ReconcileWrapper : IReconciler
    {
        private readonly Func<ReconcileContext,ReconcileRequest,Task<ReconcileResult>> _reconciler;
        public ReconcileWrapper(Func<ReconcileContext,ReconcileRequest,Task<ReconcileResult>> func)
        {
            _reconciler = func;
        }

        public async Task<ReconcileResult> Reconcile(
            ReconcileContext context, ReconcileRequest request)
        {
            return await _reconciler(context, request);
        }
    }
}
