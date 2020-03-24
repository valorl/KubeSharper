using System;
using System.Collections.Generic;
using System.Text;

namespace KubeSharper.EventSources
{
    public interface IEventSourceHandlerProvider
    {
        EventSourceHandler New();
    }
}
