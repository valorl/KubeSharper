using k8s;
using k8s.Models;
using KubeSharper.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace KubeSharper.EventSources
{
    public interface IEventSources
    {
        EventSource<T> GetNamespacedFor<T>(IKubernetes operations, string @namespace, CancellationToken ct = default);
        EventSource<CustomResource> GetNamespacedForCustom<T>(IKubernetes operations, string @namespace, CancellationToken ct = default);
    }
}
