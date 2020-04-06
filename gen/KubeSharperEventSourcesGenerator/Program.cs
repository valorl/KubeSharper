using k8s;
using Scriban;
using System;
using System.Collections.Generic;
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

			var eventSourcesFile = "EventSources.cs";

			var outputDir = args.ElementAtOrDefault(1)
				?? Path.GetFullPath( @"..\..\..\..\..\src\KubeSharper.EventSources\generated");

			await Generate("EventSources.cs", outputDir, GetEventSourceResources());
		}

		private async static Task Generate(
			string fileName, string outputDir, IEnumerable<dynamic> resources)
		{
			var templatePath = Path.GetFullPath(fileName + ".sbn");
			var template = Template.Parse(await File.ReadAllTextAsync(templatePath));
			var rendered = await template.RenderAsync(new { Resources = resources });

			var outputPath = Path.Join(Path.GetFullPath(outputDir), fileName);
			await File.WriteAllTextAsync(outputPath, rendered, Encoding.UTF8);
		}
		
		private static IEnumerable<dynamic> GetEventSourceResources()
		{
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

			
			return typeof(IKubernetes)
				.GetMethods().Where(Filter).Select(lister => new
			{
				HttpMessageLister = lister.Name,
				Lister = lister.Name.Replace("WithHttpMessages", string.Empty),
				Type = GetResourceType(lister)
			});
		}

		//private static IEnumerable<dynamic> GetListerResources()
		//{
		//	bool Filter(MethodInfo mi)
		//	{
		//		var n = mi.Name;
		//		return !n.Contains("WithHttpMessagesAsync")
		//			&& n.StartsWith("ListNamespaced")
		//			&& n.EndsWith("Async")
		//			&& !n.Contains("Object");
		//	}
			
		//	string GetResourceType(MethodInfo lister)
		//	{
		//		return lister.ReturnType
		//			.GenericTypeArguments
		//			.First().Name[0..^4];
		//	}

		//	return typeof(KubernetesExtensions)
		//		.GetMethods().Where(Filter).Select(lister => new
		//	{
		//		Lister = lister.Name,
		//		Type = GetResourceType(lister)
		//	});
		//}


		
	}
}
