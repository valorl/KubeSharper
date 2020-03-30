using System;
using System.Collections.Generic;
using System.Text;

namespace KubeSharper.Utils
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class CustomResourceDefinitionAttribute : Attribute
    {
        public string Group { get; set; }
        public string Version { get; set; }
        public string Plural { get; set; }
        public string Singular { get; set; }
        public string[] ShortNames { get; set; }

        public CustomResourceDefinitionAttribute(
            string group,
            string version,
            string plural,
            string singular,
            string[] shortNames = null)
        {
            Group = group;
            Version = version;
            Plural = plural;
            Singular = singular;
            ShortNames = shortNames;
        }
    }
}
