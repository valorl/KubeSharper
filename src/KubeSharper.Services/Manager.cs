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
        internal IEventSources EventSources { get; }

        private CancellationTokenSource _cts;

        private readonly List<IStartable> _startables = new List<IStartable>();

        public static Manager CreateAsync(KubernetesClientConfiguration kubeConfig)
        {
            return new Manager(kubeConfig);
        }
        private Manager(KubernetesClientConfiguration kubeConfig)
           : this(new Kubernetes(kubeConfig), new EventSources.EventSources()) {}

        internal Manager(IKubernetes client, IEventSources sources)
        {
            Client = client;
            EventSources = sources;

            _cts = new CancellationTokenSource();
        }

        public void Start(CancellationToken ct = default)
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
                    Log.Error(ex, $"[Startable: {id}] Exception thrown");
                }
                Log.Information($"[Startable {id}] Finished");
            }

            var tasks = _startables.Select(s =>
                s.Start(_cts.Token)
                .ContinueWith(t => OnFinished(t, s.Id)))
                .ToList();
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
