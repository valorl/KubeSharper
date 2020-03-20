using k8s;
using k8s.Models;
using KubeSharper.Services;
using System;
using System.Threading;
using System.Threading.Tasks;
using YamlDotNet.Serialization.NamingConventions;

namespace KubeSharper
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //var wq = new WorkQueue<string>();
            //wq.Start(item => Console.WriteLine(item));
            //wq.Enqueue("test1");
            //wq.Enqueue("test2");
            //Console.ReadLine();
            var configFile = @"C:\Users\vao\kubeconfig.yaml";
            var config = KubernetesClientConfiguration.BuildConfigFromConfigFile(configFile);
            var client = new Kubernetes(config);
            //client.ListNamespacedPodWithHttpMessagesAsync();
            var res = await client.ListNamespacedCustomObjectWithHttpMessagesAsync("valorl.dev", "v1", "default", "examples", watch: true).ConfigureAwait(false);
            using var watch = res.Watch((WatchEventType t, Example o) =>
            //var res = await client.ListNamespacedCustomObjectWithHttpMessagesAsync("networking.k8s.io", "v1", "default", "networkpolicies", watch: true).ConfigureAwait(false);
            //using var watch = res.Watch((WatchEventType t, V1NetworkPolicy o) =>
            //{
            //    var yaml = new YamlDotNet.Serialization.SerializerBuilder()
            //    .WithNamingConvention(CamelCaseNamingConvention.Instance)
            //    .Build()
            //    .Serialize(o);
            //    Console.WriteLine("=====================================");
            //    Console.WriteLine($"EVENT: {t}");
            //    Console.WriteLine($"OBJECT:");
            //    Console.WriteLine(yaml);
            //    Console.WriteLine("=====================================");
            //});

            //Console.WriteLine("Press Ctrl+C to stop watching");
            //var ctrlc = new ManualResetEventSlim(false);
            //Console.CancelKeyPress += (sender, eargs) => ctrlc.Set();
            //ctrlc.Wait();


            Console.ReadLine();
        }
    }
}
