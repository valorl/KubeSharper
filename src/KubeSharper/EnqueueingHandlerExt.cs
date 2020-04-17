using KubeSharper.EventQueue;
using KubeSharper.EventSources;
using KubeSharper.Reconcilliation;
using static KubeSharper.Handlers;

namespace KubeSharper
{
    internal static class EnqueueingHandlerExt
    {
        internal static EventSourceHandler ToEventSourceHandler(
            this EnqueueingHandler handler, IEventQueue<ReconcileRequest> queue)
        {
            return (et, obj) => handler(et, obj, queue);
        }
    }
}
