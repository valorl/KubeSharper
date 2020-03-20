using System;
using System.Collections.Generic;
using System.Text;

namespace KubeSharper.Services
{
    public interface IEventQueueFactory<T>
    {
        EventQueue<T> NewEventQueue();
    }
}
