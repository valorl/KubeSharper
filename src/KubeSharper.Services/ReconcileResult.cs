using System;
using System.Collections.Generic;
using System.Text;

namespace KubeSharper.Services
{
    public class ReconcileResult
    {
        public bool Requeue { get; set; }
        public TimeSpan RequeueAfter { get; set; }
    }
}
