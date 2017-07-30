using System;
using EzBus.Core.Routing;
using Xunit;

namespace EzBus.Core.Test.Routing
{
    public class ConfigurableMessageRoutingTest
    {
        private static readonly ConfigurableMessageRouting routing = new ConfigurableMessageRouting();

        [Fact]
        public void DestinationMissingException_should_be_thrown_when_missing_route()
        {
            void GetRoute()
            {
                routing.GetRoute("Unknown.Assembly", "UnknownMessage");
            }

            Assert.Throws<DestinationMissingException>((Action)GetRoute);
        }

        [Fact]
        public void Correct_route_should_be_resolved_when_only_namespace_configured()
        {
            var route = routing.GetRoute("Globex.Commands", "AnyMessage");

            Assert.Equal("globex.endpoint", route);
        }

        [Fact]
        public void Correct_route_should_be_resolved_when_namespace_and_message_type_configured()
        {
            var route = routing.GetRoute("Globex.Commands", "PlaceOrder");

            Assert.Equal("globex.endpoint.fastlane", route);
        }
    }
}
