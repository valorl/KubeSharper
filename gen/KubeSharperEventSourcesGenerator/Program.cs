using k8s;
using Scriban;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace KubeSharperEventSourcesGenerator
{
    class Program
    {
        async static Task Main(string[] args)
        {

			//var templatePath = args[1];
			//var outputPath = args[2];

			var templatePath = "EventSources.cs.sbn";
			var outputPath = @"C:\Repos\KubeSharper\src\KubeSharper.EventSources\generated\EventSources.cs";
			
			bool Filter(MethodInfo mi)
			{
				var n = mi.Name;
				return n.StartsWith("ListNamespaced")
					&& n.EndsWith("WithHttpMessagesAsync")
					&& !n.Contains("Object");
			}

			string GetResourceType(MethodInfo lister)
			{
				return lister.ReturnType
					.GenericTypeArguments
					.First()
						.GenericTypeArguments.
						First().Name[0..^4];
			}

			var methods = typeof(IKubernetes).GetMethods();
			var listers = methods.Where(Filter);

			var resources = listers.Select(lister => new
			{
				Lister = lister.Name,
				Type = GetResourceType(lister)
			});

			var template = Template.Parse(await File.ReadAllTextAsync(templatePath));

			var rendered = await template.RenderAsync(new { Resources = resources });

			await File.WriteAllTextAsync(outputPath, rendered);
		}
	}
}
