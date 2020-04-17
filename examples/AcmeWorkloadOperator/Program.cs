using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using k8s;
using k8s.Models;
using KubeSharper;
using KubeSharper.Reconcilliation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace AcmeWorkloadOperator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .Enrich.WithThreadId()
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss.fff} {Level:u3}] [{ThreadId}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<AcmeReconciler>();

                    services.KubeSharperManager(@"C:\Users\vao\kubeconfig.yaml")
                        .WithController((sp, cfg) =>
                        {
                            cfg.Watch<V1Secret>("default", Handlers.ObjectEnqueuer());
                            cfg.Watch<V1Pod>("kube-system", Handlers.ObjectEnqueuer());

                            cfg.Options.Reconciler = sp.GetRequiredService<AcmeReconciler>();
                            cfg.Options.ResyncPeriod = TimeSpan.FromSeconds(10);

                        })
                        .WithController((sp, cfg) =>
                        {
                            cfg.Options.Reconciler = Reconciler.Wrap(async (ctx, req) =>
                            {
                                Log.Information("Another reconciler is totally reconiling right now...");
                                return await Task.FromResult(new ReconcileResult());
                            });

                            cfg.Watch<V1Secret>("default", Handlers.ObjectEnqueuer());
                            cfg.Watch<V1Pod>("kube-system", Handlers.ObjectEnqueuer());
                        })
                        .Add();
                });
    }
}
