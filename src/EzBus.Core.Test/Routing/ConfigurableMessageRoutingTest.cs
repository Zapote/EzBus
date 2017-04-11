using System;
using System.IO;
using EzBus.Core.Routing;
using NUnit.Framework;

namespace EzBus.Core.Test.Routing
{
    [TestFixture]
    public class ConfigurableMessageRoutingTest
    {
        private static ConfigurableMessageRouting routing;

        [OneTimeSetUp]
        public void TestFixtureSetup()
        {
            Directory.SetCurrentDirectory(TestContext.CurrentContext.TestDirectory);
            routing = new ConfigurableMessageRouting();
        }

        [Test]
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
        public void Correct_route_should_be_resolved_when_only_namespace_configured()
        {
            var route = routing.GetRoute("Globex.Commands", "AnyMessage");

            Assert.That(route, Is.EqualTo("globex.endpoint"));
        }

        [Test]
        public void Correct_route_should_be_resolved_when_namespace_and_message_type_configured()
        {
            var route = routing.GetRoute("Globex.Commands", "PlaceOrder");

            Assert.That(route, Is.EqualTo("globex.endpoint.fastlane"));
        }
    }
}
