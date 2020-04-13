using KubeSharper.EventQueue;
using KubeSharper.EventSources;
using KubeSharper.Reconcilliation;
using System;
using System.Collections.Generic;
using System.Text;
using static KubeSharper.Reconcilliation.Handlers;

namespace KubeSharper.Services
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
