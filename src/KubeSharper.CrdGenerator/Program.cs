using k8s.Models;
using KubeSharper.Utils;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace KubeSharper.CrdGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var assemblyPath = args.ElementAtOrDefault(1) ?? @"../../../../../examples/AcmeWorkloadOperator/bin/Debug/netcoreapp3.1/AcmeWorkloadOperator.dll";
            var outputPath = args.ElementAtOrDefault(2) ?? @"../../../../../examples/AcmeWorkloadOperator/yaml";
            var assembly = Assembly.LoadFrom(assemblyPath);
            var types = assembly.GetTypes();

            var typeCrds = types
                .Select(t => (Type: t, Crd: CustomResourceDefinition.For(t)))
                .Where(tuple => tuple.Crd != null);

            foreach(var (type, crd) in typeCrds)
            {
                var spec = type.GetProperty("Spec");
                var specSchema = GetSchemaFor(spec.PropertyType);
                var specSchemaObj = specSchema.ToObjectGraph(); 

                var status = type.GetProperty("Status");
                var statusSchema = GetSchemaFor(status.PropertyType);
                var statusSchemaObj = statusSchema.ToObjectGraph();

                var parent = new
                {
                    openAPIV3Schema = new
                    {
                        type = "object",
                        properties = new
                        {
                            spec = specSchemaObj,
                            status = statusSchemaObj
                        }
                    }
                };

                var crdModel = new Crd
                {
                    Metadata = new V1ObjectMeta
                    {
                        Name = $"{crd.Plural}.{crd.Group}"
                    },
                    Spec = new CrdSpec
                    {
                        Group = crd.Group,
                        Names = new CrdNames
                        {
                            Kind = type.Name,
                            Singular = crd.Singular,
                            Plural = crd.Plural,
                            ShortNames = crd.ShortNames
                        },
                        Versions = new[]
                        {
                            new CrdVersion
                            {
                                Name = crd.Version,
                                Served = true,
                                Storage = true,
                                Schema = parent
                            }
                        }
                    }
                };



                var serializer = new SerializerBuilder()
                    .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull)
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .Build();
                var yaml = serializer.Serialize(crdModel);
                File.WriteAllText(Path.Join(outputPath, $"{crd.Singular}.yaml"), yaml);
            }

        }

        static string SerializeSchema(OpenApiSchema schema)
        {
            var str = schema.Serialize(OpenApiSpecVersion.OpenApi3_0, OpenApiFormat.Yaml);
            return str;
        }

        static JSchema GetSchemaFor(Type t)
        {
            var gen = new JSchemaGenerator
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DefaultRequired = Newtonsoft.Json.Required.DisallowNull,
                SchemaReferenceHandling = SchemaReferenceHandling.None
            };
            var js = gen.Generate(t);
            return js;
        }
    }
}
