using System;
using EzBus.Core.Test.TestHelpers;
using EzBus.Logging;
using NUnit.Framework;

namespace EzBus.Core.Test.Specifications
{
    [Specification]
    public class When_message_is_received_and_exception_is_thrown : SpecificationBase, IHandle<FailingMessage>
    {
        private FakeMessageChannel messageChannel;
        private Host host;
        private IBus bus;
        private static int retries = 0;

        protected override void Given()
        {
            retries = 0;

            FakeMessageChannel.Reset();
            messageChannel = new FakeMessageChannel();
            bus = new CoreBus(messageChannel, new FakeMessageRouting(), new InMemorySubscriptionStorage());
            var config = new HostConfig();
            config.SetNumberOfRetrys(2);
            config.ObjectFactory.Register<ISubscriptionStorage>(new InMemorySubscriptionStorage(), LifeCycle.Unique);

            host = new Host(config, new FakeMessageChannelResolver());
            host.Start();

            HostLogManager.SetLogLevel(LogLevel.Off);
        }

        protected override void When()
        {
            bus.Send("Moon", new FailingMessage());
        }

        [Test]
        public void Message_should_be_retried_two_times()
        {
            Assert.That(retries, Is.EqualTo(2));
        }

        [Test]
        public void Message_should_be_placed_on_error_queue()
        {
            Assert.That(FakeMessageChannel.LastSentDestination.QueueName, Is.EqualTo("ezbus.core.error"));
        }

        public void Handle(FailingMessage message)
        {
            retries++;
            throw new Exception("Testing error in handler.");
        }
    }
}