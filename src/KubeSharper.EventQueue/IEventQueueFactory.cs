using System;
using System.Collections.Generic;
using System.Text;

namespace KubeSharper.EventQueue
{
    public interface IEventQueueFactory<T>
    {
        IEventQueue<T> NewEventQueue();
    }
}
