using System;
using System.Threading;
using System.Threading.Tasks;

namespace KubeSharper
{
    public interface IStartable : IDisposable
    {
        string Id { get; }
        Task Start(CancellationToken ct = default);
    }
}
