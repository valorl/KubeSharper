using k8s;
using k8s.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace KubeSharper.EventSources
{
    public interface IEventSources
    {
        EventSource<T> GetNamespacedFor<T>(IKubernetes operations, string @namespace);
        //EventSource<V1Pod> V1Pod(IKubernetes operations, string @namespace);
        //EventSource<V1Secret> V1Secret(IKubernetes operations, string @namespace);
    }
}
