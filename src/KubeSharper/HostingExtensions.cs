using k8s;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static KubeSharper.Handlers;

namespace KubeSharper
{
    public static class HostingExtensions
    {
        public static ManagerConfiguration AddKubeSharperManager(this IServiceCollection services, KubernetesClientConfiguration kubeConfig)
        {
            var mgr = Manager.Create(kubeConfig).ConfigureAwait(false).GetAwaiter().GetResult();
            services.AddSingleton(mgr);
            services.AddHostedService(sp => new HostedManager(sp.GetRequiredService<Manager>()));
            return new ManagerConfiguration(mgr);
        }
        public static ManagerConfiguration AddKubeSharperManager(this IServiceCollection services, string kubeConfigPath)
        {
            var config = KubernetesClientConfiguration.BuildConfigFromConfigFile(kubeConfigPath);
            return AddKubeSharperManager(services, config);
        }
        //public static IServiceCollection AddKubeSharperController(this IServiceCollection services, Action<ControllerConfiguration> configurator)
        //{
        //    return services.AddSingleton(sp =>
        //    {
        //        var manager = sp.GetRequiredService<Manager>();

        //        var config = new ControllerConfiguration(new ControllerOptions());
        //        configurator(config);

        //        var controller = new Controller(manager, config.Options);
        //        foreach(var adder in config.WatchAdders)
        //        {
        //            adder(controller);
        //        }
        //        return controller;
        //    });
        //}

    }

    public class HostedManager : IHostedService, IDisposable
    {
        private readonly Manager _manager;
        public HostedManager(Manager manager)
        {
            _manager = manager;
        }
        public Task StartAsync(CancellationToken cancellationToken) => _manager.Start(cancellationToken);

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
        public void Dispose() => _manager.Dispose();
    }


    public class ManagerConfiguration
    {
        private readonly Manager _mgr;
        public ManagerConfiguration(Manager mgr)
        {
            _mgr = mgr;
        }

        public ManagerConfiguration WithController(Action<ControllerConfiguration> configurator)
        {
            var config = new ControllerConfiguration(new ControllerOptions());
            configurator(config);

            var controller = new Controller(_mgr, config.Options);
            foreach(var adder in config.WatchAdders)
            {
                adder(controller);
            }
            return this;
        } 

    }

    public class ControllerConfiguration
    {
        internal readonly IList<Action<Controller>> WatchAdders = new List<Action<Controller>>();
        public ControllerOptions Options { get; }
        public ControllerConfiguration(ControllerOptions opts)
        {
            Options = opts;
        }

        public void Watch<T>(string @namespace, EnqueueingHandler handler)
        {
            WatchAdders.Add(ctrl => ctrl.AddWatch<T>(@namespace, handler));
        }
    } 
}
