using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace KubeSharper.CrdGenerator
{
    public static class JSchemaExtensions
    {
        public static string ToYaml(this JSchema schema)
        {
            var schemaObj = schema.ToObjectGraph();
            var serializer = new YamlDotNet.Serialization.Serializer();
            return serializer.Serialize(schemaObj);
        }

        public static object ToObjectGraph(this JSchema schema)
        {
            var json = schema.ToString();
            var expConverter = new ExpandoObjectConverter();
            dynamic schemaObj = JsonConvert.DeserializeObject<ExpandoObject>(json, expConverter);
            return schemaObj;
        }
    }
}
