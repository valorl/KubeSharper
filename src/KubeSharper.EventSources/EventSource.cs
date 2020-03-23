using k8s;
using KubeSharper.EventQueue;
using KubeSharper.Reconcilliation;
using Microsoft.CSharp.RuntimeBinder;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KubeSharper.EventSources
{
    public class EventSource<T>
    {
        private readonly Func<Action<WatchEventType, T>, Task<Watcher<T>>> _watchMaker;

        private Watcher<T> _watcher;
        public EventSource(Func<Action<WatchEventType, T>, Task<Watcher<T>>> watchMaker)
        {
            _watchMaker = watchMaker;
        }


        public async Task Start(EventQueue<ReconcileRequest> queue)
        {
            _watcher = await _watchMaker(async (et, obj) =>
            {
                try
                {
                    dynamic d = obj;
                    var req = new ReconcileRequest()
                    {
                        ApiVersion = d.ApiVersion,
                        Kind = d.Kind,
                        Namespace = d.Metadata.NamespaceProperty,
                        Name = d.Metadata.Name
                    };
                    if (!(await queue.TryAdd(req)))
                    {
                        Log.Error($"Failed adding {req.ApiVersion}/{req.Namespace}/{req.Kind}/{req.Name}");
                    }
                    else
                    {
                        Log.Information($"Added {req}");
                    }
                }
                catch(Exception ex)
                {
                    Log.ForContext("Object", obj).Error(ex, "Handler failed.");
                }
            }).ConfigureAwait(false);
        }
    }

}
