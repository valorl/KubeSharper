using k8s;
using k8s.Models;
using System;

namespace KubeSharper.Utils
{
    public class KubernetesV1MetaObject : KubernetesObject
    {
        public V1ObjectMeta Metadata { get; set; }
    }

}
