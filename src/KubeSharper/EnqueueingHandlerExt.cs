using KubeSharper.WorkQueue;
using KubeSharper.EventSources;
using KubeSharper.Reconcilliation;
using static KubeSharper.Handlers;

namespace KubeSharper
{
    internal static class EnqueueingHandlerExt
    {
        internal static EventSourceHandler ToEventSourceHandler(
            this EnqueueingHandler handler, IWorkQueue<ReconcileRequest> queue)
        {
            return (et, obj) => handler(et, obj, queue);
        }
    }
}
