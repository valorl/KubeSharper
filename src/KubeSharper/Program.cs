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
                .MinimumLevel.Verbose()
                .Enrich.WithThreadId()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss.fff} {Level:u3}] [{ThreadId}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            var cts = new CancellationTokenSource();
            var configFile = @"C:\Users\vao\kubeconfig.yaml";
            var config = KubernetesClientConfiguration.BuildConfigFromConfigFile(configFile);
            var client = new Kubernetes(config);

            var secrets = await client.ListNamespacedSecretWithHttpMessagesAsync("default");

            var manager = Manager.CreateAsync(config);


            var controller = new Controller(manager, (ctx, req) =>
            {
                Log.Information($"{req}");
                return Task.FromResult(new ReconcileResult());
            });

            var resync = TimeSpan.FromSeconds(10);
            controller.AddWatch<V1Secret>("default", Handlers.ObjectEnqueuer(), resync);
            controller.AddWatch<V1Pod>("kube-system", Handlers.ObjectEnqueuer(), resync);

            //await controller.Start(cts.Token);

            manager.Start(cts.Token);


            var ctrlc = new ManualResetEventSlim(false);
            Console.CancelKeyPress += (sender, eventArgs) => {
                eventArgs.Cancel = true;
                manager.Dispose();
                ctrlc.Set();
            };
            ctrlc.Wait();
            Console.WriteLine();
        }
    }
}
