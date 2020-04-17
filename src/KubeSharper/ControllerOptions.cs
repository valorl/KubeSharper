using KubeSharper.Reconcilliation;
using System;

namespace KubeSharper
{
    public class ControllerOptions
    {
        public IReconciler Reconciler { get; set; }
        public TimeSpan ResyncPeriod { get; set; } = TimeSpan.FromHours(10);
    }
}
