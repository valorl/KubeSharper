using k8s;
using KubeSharper.EventSources;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KubeSharper
{
    public interface IManager : IDisposable
    {
        void Add(IStartable startable);
        Task Start(CancellationToken cancellationToken = default);
        public IKubernetes Client { get;}
    }
}
