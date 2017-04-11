using System;
using System.IO;
using EzBus.Config;
using EzBus.Core.Config;
using NUnit.Framework;

namespace EzBus.Core.Test.Config
{
    [TestFixture]
    public class EzBusConfigTest
    {
        private static readonly IEzBusConfig config = EzBusConfig.GetConfig();

        [OneTimeSetUp]
        public void TestFixtureSetup()
        {
            Directory.SetCurrentDirectory(TestContext.CurrentContext.TestDirectory);
        }

        [Test]
        public void EndpointName_should_be_EzBus_Endpoint()
        {
            Assert.That(config.EndpointName, Is.EqualTo("ezbus.endpoint"));
        }

        [Test]
        public void First_destination_should_be_acme_input()
        {
            var destination = config.Destinations[0];

            Assert.That(destination.Endpoint, Is.EqualTo("acme.endpoint"));
            Assert.That(destination.Namespace, Is.EqualTo("acme.commands"));
            Assert.That(destination.Message, Is.Null);
        }

        [Test]
        public void Second_destination_should_be_globex_input()
        {
            var destination = config.Destinations[1];

            Assert.That(destination.Endpoint, Is.EqualTo("globex.endpoint"));
            Assert.That(destination.Namespace, Is.EqualTo("globex.commands"));
            Assert.That(destination.Message, Is.Null);
        }

        [Test]
        public void First_subscription_should_be_globex_endpoint()
        {
            var destination = config.Subscriptions[0];

            Assert.That(destination.Endpoint, Is.EqualTo("globex.endpoint"));
        }

        [Test]
        public void Second_subscription_should_be_acme_endpoint()
        {
            var destination = config.Subscriptions[1];

            Assert.That(destination.Endpoint, Is.EqualTo("acme.endpoint"));
        }
    }
}
