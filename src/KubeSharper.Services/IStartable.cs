using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KubeSharper.Services
{
    public interface IStartable : IDisposable
    {
        string Id { get; }
        Task Start(CancellationToken ct = default);
    }
}
