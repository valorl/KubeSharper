using k8s;
using System;
using System.Collections.Generic;
using System.Text;

namespace KubeSharper.EventSources
{
    public interface IListers
    {
        Lister<T> GetNamespacedFor<T>(IKubernetes operations);
        Lister<T> GetNamespacedForCustom<T>(IKubernetes operations);
    }
}
