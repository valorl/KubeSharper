using k8s;
using k8s.Models;
using KubeSharper.EventQueue;
using KubeSharper.EventSources;
using KubeSharper.Reconcilliation;
using KubeSharper.Services;
using Serilog;
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
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            var cts = new CancellationTokenSource();
            var configFile = @"C:\Users\vao\kubeconfig.yaml";
            var config = KubernetesClientConfiguration.BuildConfigFromConfigFile(configFile);
            var client = new Kubernetes(config);

            var q = new EventQueue<ReconcileRequest>();
            var first = await q.TryAdd(new ReconcileRequest
            {
                ApiVersion = "v1",
                Kind = "Secret",
                Namespace = "default",
                Name = "dev-db-secret"
            });
            var second = await q.TryAdd(new ReconcileRequest
            {
                ApiVersion = "v1",
                Kind = "Secret",
                Namespace = "default",
                Name = "dev-db-secret"
            });
            Console.WriteLine($"first {first}, second {second}");
            _ = Task.Run(async () =>
              {
                  while (true)
                  {
                      if (q.TryGet(out var r))
                      {
                          Log.Information($"Got {r}");
                      }
                      await Task.Delay(100, cts.Token);
                  }
              }, cts.Token).ConfigureAwait(false);

            var sources = new EventSources.EventSources();
            var source = sources.GetNamespacedFor<V1Secret>(client, "default");
            await source.Start(q).ConfigureAwait(false);
            await Task.Delay(100);


            Console.WriteLine("Press Ctrl+C to stop watching");
            var ctrlc = new ManualResetEventSlim(false);
            Console.CancelKeyPress += (sender, eargs) => { ctrlc.Set(); cts.Cancel(); };
            ctrlc.Wait();
        }
    }
}
