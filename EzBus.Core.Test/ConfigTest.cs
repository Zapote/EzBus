using System.Reflection;
using Xunit;

namespace EzBus.Core.Test
{
    public class ConfigTest
    {
        private IConfig config = new Config();

        [Fact]
        public void EndpointName_should_be_applicitaionName()
        {
            var expected = $"{Assembly.GetEntryAssembly().GetName().Name.Replace(".", "-")}";

            Assert.Equal(expected, config.Address);
        }

        [Fact]
        public void ErrorEndpointName_should_be_applicitaionName_plus_error()
        {
            var expected = $"{Assembly.GetEntryAssembly().GetName().Name.Replace(".", "-")}-error";

            Assert.Equal(expected, config.ErrorAddress);
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
