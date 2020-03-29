using System;
using System.Collections.Generic;
using System.Text;

namespace KubeSharper.Reconcilliation
{
    public class ReconcileResult
    {
        public bool Requeue { get; }
        public TimeSpan RequeueAfter { get; }

        public ReconcileResult() : this(false) {}
        public ReconcileResult(bool requeue) : this(requeue, TimeSpan.Zero) { }
        public ReconcileResult(TimeSpan requeueAfter) : this(true, requeueAfter) { }

        private ReconcileResult(bool requeue, TimeSpan requeueAfter)
        {
            Requeue = requeue;
            RequeueAfter = requeueAfter;
        }
    }
}
