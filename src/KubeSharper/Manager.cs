using k8s;
using KubeSharper.EventSources;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KubeSharper
{
    public class Manager
    {
        public IKubernetes Client { get;}
        internal IEventSourceCache Cache { get; }

        private CancellationTokenSource _cts;

        private readonly List<IStartable> _startables = new List<IStartable>();

        internal static async Task<Manager> Create(KubernetesClientConfiguration kubeConfig)
        {
            var sources = new EventSources.EventSources();
            var client = new Kubernetes(kubeConfig);
            var cache = new EventSourceCache(sources, client);
            return await Task.FromResult(new Manager(client, cache));
        }
        public static async Task<Manager> Create(string kubeConfigPath)
        {
            var fi = new FileInfo(kubeConfigPath);
            var config = await KubernetesClientConfiguration.BuildConfigFromConfigFileAsync(fi);
            return await Create(config);
        }
        internal Manager(IKubernetes client, IEventSourceCache cache)
        {
            Client = client;
            Cache = cache;

            _cts = new CancellationTokenSource();
        }

        public async Task Start(CancellationToken ct = default)
        {
            _cts = (ct == CancellationToken.None) switch
            {
                true => new CancellationTokenSource(),
                false => CancellationTokenSource.CreateLinkedTokenSource(ct)
            };

            void OnFinished(Task task, string id)
            {
                var ex = task.Exception;
                if(ex != null && ex.InnerException != null)
                {
                    Log.Error(ex, $"[Manager] Exception when starting {id}");
                }
                Log.Information($"[Manager] Finished starting {id}");
            }

            // Have all controllers subscribe to all watches and start their loops
            var tasks = _startables.Select(s =>
                s.Start(_cts.Token)
                .ContinueWith(t => OnFinished(t, s.Id)))
                .ToList();
            await Task.WhenAll(tasks);

            //Now start the event sources
            Cache.StartAll();
        }

        public void Add(IStartable startable) => _startables.Add(startable);

        public void Dispose()
        {
            Cache.Dispose();
            foreach (var s in _startables)
            {
                s.Dispose();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Log.Debug("Manager is stopping.");
            return Task.CompletedTask;
        }
    }
}
