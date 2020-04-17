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
        public static ManagerConfiguration KubeSharperManager(this IServiceCollection services, KubernetesClientConfiguration kubeConfig)
        {
            var mgr = Manager.Create(kubeConfig).ConfigureAwait(false).GetAwaiter().GetResult();
            //services.AddSingleton(mgr);
            //services.AddHostedService(sp => new HostedManager(sp.GetRequiredService<Manager>()));
            return new ManagerConfiguration(mgr, services);
        }
        public static ManagerConfiguration KubeSharperManager(this IServiceCollection services, string kubeConfigPath)
        {
            var config = KubernetesClientConfiguration.BuildConfigFromConfigFile(kubeConfigPath);
            return KubeSharperManager(services, config);
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
        private readonly IServiceCollection _services;
        private readonly List<Action<IServiceProvider, ControllerConfiguration>> _configurators =
            new List<Action<IServiceProvider, ControllerConfiguration>>();
        public ManagerConfiguration(Manager mgr, IServiceCollection services)
        {
            _mgr = mgr;
            _services = services;
        }

        public ManagerConfiguration WithController(Action<IServiceProvider, ControllerConfiguration> configurator)
        {
            _configurators.Add(configurator);
            //var config = new ControllerConfiguration(new ControllerOptions());
            //configurator(config);

            //var controller = new Controller(_mgr, config.Options);
            //foreach(var adder in config.WatchAdders)
            //{
            //    adder(controller);
            //}
            return this;
        } 

        public IServiceCollection Add()
        {
            _services.AddHostedService(sp =>
            {
                foreach(var configurator in _configurators)
                {
                    var config = new ControllerConfiguration(new ControllerOptions());
                    configurator(sp, config);

                    var controller = new Controller(_mgr, config.Options);
                    foreach (var adder in config.WatchAdders)
                    {
                        adder(controller);
                    }
                }
                return new HostedManager(_mgr);

            });
            return _services;
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
