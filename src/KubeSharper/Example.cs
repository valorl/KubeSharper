using KubeSharper.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace KubeSharper
{
    public class Example : CustomResource<ExampleSpec,ExampleStatus> {}

    public class ExampleSpec
    {
        public string ExampleTitle { get; set; }
        public string ExampleText { get; set; }
    }
    public class ExampleStatus
    {
        public bool HasTitle { get; set; }
        public bool HasText { get; set; }
    }
}
