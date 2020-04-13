using k8s;
using System;
using System.Collections.Generic;
using System.Text;

namespace KubeSharper.Utils
{
    public enum EventType
    {
        Added,
        Modified,
        Deleted,
        Error,
        Resync
    }

    public static class WatchEventTypeExt
    {
        public static EventType ToInternal(this WatchEventType et) => et switch
        {
            WatchEventType.Added => EventType.Added,
            WatchEventType.Modified => EventType.Modified,
            WatchEventType.Deleted => EventType.Deleted,
            WatchEventType.Error => EventType.Error,
        };
    }
}
