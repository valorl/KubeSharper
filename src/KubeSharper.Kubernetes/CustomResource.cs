using k8s;
using k8s.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace KubeSharper.Utils
{
    public class CustomResource : KubernetesObject
    {
        public V1ObjectMeta Metadata { get; set; }
    }

    public abstract class CustomResource<TSpec, TStatus> : CustomResource
        where TSpec : new()
        where TStatus : new()
        
    {
        public TSpec Spec { get; set; }
        public TStatus Status { get; set; }
    }

    public abstract class CustomResourceList<T> : KubernetesObject where T : CustomResource
    {
        public V1ListMeta Metadata { get; set; }
        public List<CustomResource> Items { get; set; }
    }
}
