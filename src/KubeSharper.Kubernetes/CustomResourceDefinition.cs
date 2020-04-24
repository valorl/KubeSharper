using System;
using System.Collections.Generic;
using System.Text;

namespace KubeSharper.Utils
{
    public class CustomResourceDefinition
    {
        public string Group { get; set; }
        public string Version { get; set; }
        public string Plural { get; set; }
        public string Singular { get; set; }
        public string[] ShortNames { get; set; }

        public static CustomResourceDefinition For(Type t)
        {
            var attr = (CustomResourceDefinitionAttribute)
                Attribute.GetCustomAttribute(t, typeof(CustomResourceDefinitionAttribute));
            if (attr == null) return null;

            return new CustomResourceDefinition
            {
                Group = attr.Group,
                Version = attr.Version,
                Plural = attr.Plural,
                Singular = attr.Singular,
                ShortNames = attr.ShortNames
            };
        }
        public static CustomResourceDefinition For<T>() => For(typeof(T));
    }
}
