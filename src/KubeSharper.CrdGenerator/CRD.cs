using k8s;
using k8s.Models;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace KubeSharper.CrdGenerator
{
    public class Crd : IMetadata<V1ObjectMeta>
    {
        public Crd()
        {
            ApiVersion = "apiextensions.k8s.io/v1";
            Kind = "CustomResourceDefinition";
        }

        public string ApiVersion { get; set; }
        public string Kind { get; set; }
        public V1ObjectMeta Metadata { get; set; }
        public CrdSpec Spec { get; set; }
    }

    public class CrdNames
    {
        public string Plural { get; set; }
        public string Singular { get; set; }
        public string Kind { get; set; }
        public string[] ShortNames { get; set; }
    }

    public enum CrdScope
    {
        Namespaced, Cluster
    }
    public class CrdSpec
    {
        public string Group { get; set; }
        public CrdNames Names { get; set; }
        public CrdScope Scope { get; set; }
        public CrdVersion[] Versions { get; set; }
    }

    public class CrdVersion
    {
        public string Name { get; set; }
        public bool Served { get; set; }
        public bool Storage { get; set; }
        public object Schema { get; set; }
    }

}
