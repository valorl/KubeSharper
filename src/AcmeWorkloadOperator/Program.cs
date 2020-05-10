using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcmeWorkloadOperator.Acme;
using AcmeWorkloadOperator.CloudRedis;
using Google.Cloud.Redis.V1;
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

                    services.AddTransient(_ =>
                    {
#if DEBUG
                        Environment.SetEnvironmentVariable(
                            "GOOGLE_APPLICATION_CREDENTIALS",
                            @"C:\Users\vao\kubesharper-18da497d6cb7.json");
#endif

                        return CloudRedisClient.Create();
                    });
                    services.AddTransient(_ => new RedisInstanceManagerSettings
                    {
                        GoogleProjectId = "kubesharper",
                        GoogleRegion = "europe-west3"
                    });
                    services.AddTransient<RedisInstanceManager>();
                    services.AddSingleton<CloudRedisReconciler>();

                    services.KubeSharperManager(@"C:\Users\vao\kubeconfig.yaml")
                        .WithController((sp, cfg) =>
                        {
                            var ns = "default";
                            cfg.Watch<V1Deployment>(ns, Handlers.EnqueueForOwner(isController: true));
                            cfg.Watch<V1Service>(ns, Handlers.EnqueueForOwner(isController: true));
                            cfg.Watch<Networkingv1beta1Ingress>(ns, Handlers.EnqueueForOwner(isController: true));
                            cfg.Watch<AcmeService>(ns, Handlers.EnqueueForObject());

                            cfg.Options.Reconciler = sp.GetRequiredService<AcmeReconciler>();
                            //cfg.Options.ResyncPeriod = TimeSpan.FromSeconds(10);

                        })
                        .WithController((sp, cfg) =>
                        {
                            var ns = "default";
                            cfg.Watch<AcmeRedis>(ns, Handlers.EnqueueForObject());
                            cfg.Options.Reconciler = sp.GetRequiredService<CloudRedisReconciler>();
                        })
                        .Add();
                });
    }
}
