using System;
using EzBus.Core.Routing;
using NUnit.Framework;

namespace EzBus.Core.Test.Routing
{
    [TestFixture]
    public class ConfigurableMessageRoutingTest
    {
        private readonly ConfigurableMessageRouting routing = new ConfigurableMessageRouting();

        public void DestinationMissingException_should_be_thrown_when_missing_route()
        {
            try
            {
                routing.GetRoute("Unknown.Assembly", "UnknownMessage");
            }
            catch (DestinationMissingException)
            {
                return;
            }
            Assert.Fail("DestinationMissingException should be thrown");
        }

        [Test]
        public void Correct_route_should_be_resolved_when_only_assembly_configured()
        {
            var route = routing.GetRoute("Acme.Messages", "MyMessage");

            Assert.That(route, Is.EqualTo("acme.input"));
        }

        [Test]
        public void Correct_route_should_be_resolved_when_assembly_and_message_type_configured()
        {
            var route = routing.GetRoute("Acme.Messages", "Acme.Messages.DoAction");

            Assert.That(route, Is.EqualTo("acme.input.fastlane"));
        }
    }
}
