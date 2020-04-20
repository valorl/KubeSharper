using k8s;
using k8s.Models;
using KubeSharper.Reconcilliation;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KubeSharper.Playground
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

            var manager = await Manager.Create(configFile);

            var controller = new Controller(manager, new ControllerOptions
            {
                //ResyncPeriod = TimeSpan.FromSeconds(10),
                Reconciler = Reconciler.Wrap((ctx, req) =>
                {
                    Log.Information($"{req}");
                    return Task.FromResult(new ReconcileResult());
                })
            }); ;

            controller.AddWatch<V1Secret>("default", Handlers.EnqueueForObject());
            controller.AddWatch<V1Pod>("kube-system", Handlers.EnqueueForObject());

            //await controller.Start(cts.Token);

            await manager.Start(cts.Token);


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
