using System;
using System.Collections.Generic;
using System.Text;

namespace KubeSharper.Services
{
    public class ReconcileRequest
    {
        public string Namespace { get; set; }
        public string Name { get; set; }
    }
}
