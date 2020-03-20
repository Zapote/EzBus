using System.Reflection;
using Xunit;

namespace EzBus.Core.Test
{
    public class ConfigTest
    {
        private readonly IBusConfig config = new BusConfig("acme-svc");

        [Fact]
        public void Address_should_be_given_address_to_lower()
        {
            Assert.Equal("acme-svc", config.Address);
        }

        [Fact]
        public void ErrorEndpointName_should_be_applicitaionName_plus_error()
        {
            Assert.Equal("acme-svc-error", config.ErrorAddress);
        }

        [Fact]
        public void NumberOfRetries_should_be_five_default()
        {
            Assert.Equal(5, config.NumberOfRetries);
        }

        [Fact]
        public void WorkerThreads_should_be_one_default()
        {
            Assert.Equal(1, config.WorkerThreads);
        }
    }
}
