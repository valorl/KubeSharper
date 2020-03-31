using k8s;
using Scriban;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KubeSharperEventSourcesGenerator
{
    class Program
    {
        async static Task Main(string[] args)
        {

			var templatePath = args.ElementAtOrDefault(1) ?? Path.GetFullPath("EventSources.cs.sbn");
			var outputPath = args.ElementAtOrDefault(2)
				?? Path.GetFullPath( @"..\..\..\..\..\src\KubeSharper.EventSources\generated\EventSources.cs");
			
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

			await File.WriteAllTextAsync(outputPath, rendered, Encoding.UTF8);
		}
	}
}
