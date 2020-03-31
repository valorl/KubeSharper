using k8s;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KubeSharper.EventSources
{
    public class Lister<T>
    {
        private readonly Func<string, Task<IList<T>>> _lister;
        public Lister(Func<string, Task<IList<T>>> listFunc)
        {
            _lister = listFunc;
        }

        public async Task<IList<T>> List(string @namespace) =>
            await _lister(@namespace);
    }
}
