using Microsoft.Extensions.PlatformAbstractions;
using Xunit;

namespace EzBus.Core.Test
{
    public class BusConfigTest
    {
        private BusConfig busConfig = new BusConfig();

        [Fact]
        public void EndpointName_should_be_applicitaionName()
        {
            var expected = PlatformServices.Default.Application.ApplicationName;

            Assert.Equal(expected, busConfig.EndpointName);
        }

        [Fact]
        public void ErrorEndpointName_should_be_applicitaionName_plus_error()
        {
            var expected = PlatformServices.Default.Application.ApplicationName + ".error";

            Assert.Equal(expected, busConfig.ErrorEndpointName);
        }

        [Fact]
        public void NumberOfRetries_should_be_five_default()
        {
            Assert.Equal(5, busConfig.NumberOfRetries);
        }

        [Fact]
        public void WorkerThreads_should_be_one_default()
        {
            Assert.Equal(1, busConfig.WorkerThreads);
        }
    }
}
