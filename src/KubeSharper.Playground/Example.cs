using KubeSharper.Utils;

namespace KubeSharper
{
    [CustomResourceDefinition("valorl.dev", "v1", "examples", "example")]
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
