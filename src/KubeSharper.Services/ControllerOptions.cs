using KubeSharper.Reconcilliation;
using System;
using System.Collections.Generic;
using System.Text;

namespace KubeSharper.Services
{
    public class ControllerOptions
    {
        public IReconciler Reconciler { get; set; }
        public TimeSpan ResyncPeriod { get; set; } = TimeSpan.FromHours(10);
    }
}
