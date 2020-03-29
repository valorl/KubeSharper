using System;
using System.Collections.Generic;
using System.Text;

namespace KubeSharper.EventQueue
{
    public class EventQueueFactory<T> : IEventQueueFactory<T>
    {
        public IEventQueue<T> NewEventQueue()
        {
            return new EventQueue<T>();
        }
    }
}
