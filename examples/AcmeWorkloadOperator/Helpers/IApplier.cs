using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AcmeWorkloadOperator.Helpers
{
    public interface IForOwnerApplier<TOwner>
    {
        Task Apply(TOwner spec, string name, string @namespace);
    }
}
