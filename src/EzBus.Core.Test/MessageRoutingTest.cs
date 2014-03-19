using EzBus.Core.Routing;
using NUnit.Framework;

namespace EzBus.Core.Test
{
    [TestFixture]
    public class MessageRoutingTest
    {
        private readonly ConfigurableMessageRouting configurableMessageRouting = new ConfigurableMessageRouting();

        [Test]
        public void Get_route_by_assembly_returns_correct_endpoint()
        {
            var endpoint = configurableMessageRouting.GetRoute("Acme.Messages", "");
            Assert.That(endpoint, Is.EqualTo("acme.input"));
        }

        [Test]
        public void Get_route_by_assembly_and_message_type_returns_correct_endpoint()
        {
            var endpoint = configurableMessageRouting.GetRoute("Acme.Messages", "Acme.Messages.DoAction");
            Assert.That(endpoint, Is.EqualTo("acme.input.fastlane"));
        }
    }
}
