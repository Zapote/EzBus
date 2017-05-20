using System;
using System.IO;
using EzBus.Config;
using EzBus.Core.Config;
using Xunit;

namespace EzBus.Core.Test.Config
{
    public class EzBusConfigTest
    {
        private static readonly IEzBusConfig config = EzBusConfig.GetConfig();

        [Fact]
        public void EndpointName_should_be_EzBus_Endpoint()
        {
            Assert.Equal("ezbus.endpoint", config.EndpointName);
        }

        [Fact]
        public void First_destination_should_be_acme_input()
        {
            var destination = config.Destinations[0];

            Assert.Equal("acme.endpoint", destination.Endpoint);
            Assert.Equal("acme.commands", destination.Namespace);
        }

        [Fact]
        public void Second_destination_should_be_globex_input()
        {
            var destination = config.Destinations[1];

            Assert.Equal("globex.endpoint", destination.Endpoint);
            Assert.Equal("globex.commands", destination.Namespace);
            Assert.Null(destination.Message);
        }

        [Fact]
        public void First_subscription_should_be_globex_endpoint()
        {
            var destination = config.Subscriptions[0];

            Assert.Equal("globex.endpoint", destination.Endpoint);
        }

        [Fact]
        public void Second_subscription_should_be_acme_endpoint()
        {
            var destination = config.Subscriptions[1];

            Assert.Equal("acme.endpoint", destination.Endpoint);
        }
    }
}
