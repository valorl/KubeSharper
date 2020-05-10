using AcmeWorkloadOperator.Acme;
using k8s;
using k8s.Models;
using System;
using System.Threading.Tasks;
using Xunit;

namespace AcmeWorkloadOperator.E2ETests
{
    public class AcmeServiceTests : TestBase, IAsyncLifetime
    {
        private string _samplePath = @"../../../../AcmeWorkloadOperator/yaml/sample-service.yaml";
        private AcmeService _sample;

        public async Task InitializeAsync()
        {
            //_sample = await Yaml.LoadFromFileAsync<AcmeService>(path);
            //_sample.Status = new AcmeServiceStatus();

            await NewNamespace();
        }
        public async Task DisposeAsync()
        {
            await CleanupNamespace();
        }

        [Fact]
        public async Task AcmeService_Should_Create_All_Children()
        {
            //await CreateCustomObject(_sample).ConfigureAwait(false);
            await KubectlApply(_samplePath).ConfigureAwait(false);
            var depTask = Poll<V1Deployment>(_sample.Metadata.Name);
            var serviceTask = Poll<V1Service>(_sample.Metadata.Name);

            await Task.WhenAll(depTask, serviceTask).ConfigureAwait(false);
            var dep = await depTask.ConfigureAwait(false); 
            var service = await serviceTask.ConfigureAwait(false);

            // Assert stuff
            Assert.NotNull(dep);
            Assert.NotNull(service);
            AssertOwnerReference(dep);
            AssertOwnerReference(service);
        }

        private void AssertOwnerReference(IMetadata<V1ObjectMeta> obj)
        {
            var references = obj.Metadata.OwnerReferences;
            Assert.NotEmpty(references);

            var reference = references[0];
            Assert.Equal(_sample.ApiVersion, reference.ApiVersion);
            Assert.Equal(_sample.Kind, reference.Kind);
            Assert.Equal(true, reference.Controller);
            Assert.Equal(true, reference.BlockOwnerDeletion);
        }
    }
}
