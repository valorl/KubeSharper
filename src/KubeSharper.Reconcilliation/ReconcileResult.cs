using System;
using System.Collections.Generic;
using System.Text;

namespace KubeSharper.Reconcilliation
{
    public class ReconcileResult
    {
        public bool Requeue { get; set; }
        public TimeSpan RequeueAfter { get; set; }
    }
}
