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



            var controller = new Controller(client, new EventQueueFactory<ReconcileRequest>(), new EventSources.EventSources(), req =>
            {
                Log.Information($"{req}");
                return Task.FromResult(new ReconcileResult());
            });

            //controller.AddWatch<Example>("default", Handlers.ObjectEnqueuer());
            //controller.AddWatch<V1Pod>("default", Handlers.ObjectEnqueuer());
            var resync = TimeSpan.FromSeconds(10);
            controller.AddWatch<V1Secret>("default", Handlers.ObjectEnqueuer(), resync);
            await controller.Start();


            var ctrlc = new ManualResetEventSlim(false);
            Console.CancelKeyPress += (sender, eventArgs) => { controller.Dispose(); ctrlc.Set(); };
            ctrlc.Wait();
        }
    }
}
