using KubeSharper.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace KubeSharper.Reconcilliation
{
    public class ReconcileRequest
    {
        public string ApiVersion { get; set; }
        public string Kind { get; set; }
        public string Namespace { get; set; }
        public string Name { get; set; }
        public ReconcileRequest()
        {

        }
        public override string ToString()
        {
            return $"{ApiVersion}/{Namespace}/{Kind}/{Name}";
        }
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + ApiVersion.GetHashCode();
                hash = hash * 23 + Kind.GetHashCode();
                hash = hash * 23 + Namespace.GetHashCode();
                hash = hash * 23 + Name.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            var it = (ReconcileRequest)obj;
            return ApiVersion == it.ApiVersion
                && Kind == it.Kind
                && Namespace == it.Namespace
                && Name == it.Name;
        }
    }
    
}
