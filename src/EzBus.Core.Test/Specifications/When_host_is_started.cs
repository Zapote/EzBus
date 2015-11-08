using EzBus.Core.Logging;
using EzBus.Core.Resolvers;
using EzBus.Core.Subscription;
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
            LogManager.Configure(new TraceLoggerFactory(), LogLevel.All);

            var objectFactory = ObjectFactoryResolver.Get();
            objectFactory.Register<ISubscriptionStorage, InMemorySubscriptionStorage>(LifeCycle.Unique);

            var hostConfig = new HostConfig();
            var host = new Host(hostConfig, objectFactory);
            host.Start();
        }

        [Then]
        public void Subscriptions_messages_should_be_sent()
        {
            Assert.That(FakeMessageChannel.LastSentDestination, Is.EqualTo(EndpointAddress.Parse(expectedDestination)));
        }
    }
}