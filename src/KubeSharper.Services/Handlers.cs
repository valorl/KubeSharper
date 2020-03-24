using k8s;
using KubeSharper.EventQueue;
using KubeSharper.EventSources;
using KubeSharper.Kubernetes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KubeSharper.Reconcilliation
{
    public enum HandlerType
    {
        EnqueueObject,
        EnqueueOwner
    }
    public static class Handlers
    {
        public static Task<EventSourceHandler> Create(HandlerType type, IEventQueue<ReconcileRequest> queue)
        {

            

        }


    }
}
