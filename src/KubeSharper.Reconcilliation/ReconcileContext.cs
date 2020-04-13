using k8s;
using System;
using System.Collections.Generic;
using System.Text;

namespace KubeSharper.Reconcilliation
{
    public class ReconcileContext
    {
        public IKubernetes Client { get; }

        public ReconcileContext(IKubernetes client)
        {
            Client = client;
        }
    }
}
