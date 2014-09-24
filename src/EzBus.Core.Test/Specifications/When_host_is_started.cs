using EzBus.Core.Test.TestHelpers;
using NUnit.Framework;

namespace EzBus.Core.Test.Specifications
{
    [Specification]
    public class When_host_is_started : SpecificationBase
    {
        private const string expectedDestination = "acme.production";

        protected override void When()
        {
            FakeMessageChannel.Reset();
            var host = new Host(new HostConfig());
            host.Start();
        }

        [Then]
        public void Subscriptions_messages_should_be_sent()
        {
            Assert.That(FakeMessageChannel.LastSentDestination, Is.EqualTo(EndpointAddress.Parse(expectedDestination)));
        }
    }
}