using System;
using EzBus.Core.Test.TestHelpers;
using NUnit.Framework;

namespace EzBus.Core.Test
{
    [TestFixture]
    public class EndpointHostTest : IHandle<FailingMessage>
    {
        private FakeMessageChannel messageChannel;
        private Host host;
        private IBus bus;

        [SetUp]
        public void TestSetup()
        {
            messageChannel = new FakeMessageChannel();
            bus = new Bus(messageChannel, new FakeMessageRouting(), new InMemorySubscriptionStorage());
            var config = new HostConfig();
            config.SetReceivingChannel(messageChannel);
            config.SetSendingChannel(messageChannel);
            host = new Host(config);
            host.Start();
        }

        [Test]
        public void When_message_is_received_and_exception_occurs_message_should_be_placed_on_error_queue()
        {
            bus.Send("Moon", new FailingMessage());

            Assert.That(messageChannel.LastSentDestination.QueueName, Is.EqualTo("ezbus.core.error"));
        }

        public void Handle(FailingMessage message)
        {
            throw new Exception("Error in handler.");
        }
    }
}