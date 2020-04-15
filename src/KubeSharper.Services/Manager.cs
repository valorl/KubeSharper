using k8s;
using KubeSharper.EventSources;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KubeSharper.Services
{
    public class Manager : IDisposable
    {
        public IKubernetes Client { get;}
        internal IEventSourceCache Cache { get; }

        private CancellationTokenSource _cts;

        private readonly List<IStartable> _startables = new List<IStartable>();

        public static async Task<Manager> Create(KubernetesClientConfiguration kubeConfig)
        {
            var sources = new EventSources.EventSources();
            var client = new Kubernetes(kubeConfig);
            var cache = new EventSourceCache(sources, client);
            return new Manager(client, cache);
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
            await Cache.StartAll();
        }

        public void Add(IStartable startable) => _startables.Add(startable);

        public void Dispose()
        {
            foreach (var s in _startables)
            {
                s.Dispose();
            }
        }
    }
}
