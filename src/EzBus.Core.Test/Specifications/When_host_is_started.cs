using EzBus.Core.Logging;
using EzBus.Core.Test.TestHelpers;
using EzBus.Logging;
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
            HostLogManager.Configure(new TraceLoggerFactory(), LogLevel.All);
            var hostConfig = new HostConfig();
            hostConfig.ObjectFactory.Register<ISubscriptionStorage>(new InMemorySubscriptionStorage(), LifeCycle.Unique);
            var host = new Host(hostConfig);

            host.Start();
        }

        [Then]
        public void Subscriptions_messages_should_be_sent()
        {
            Assert.That(FakeMessageChannel.LastSentDestination, Is.EqualTo(EndpointAddress.Parse(expectedDestination)));
        }
    }
}