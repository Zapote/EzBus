using System;
using EzBus.Core.Test.TestHelpers;
using NUnit.Framework;

namespace EzBus.Core.Test.Specifications
{
    [Specification]
    public class When_message_is_received_and_exception_is_thrown : SpecificationBase, IHandle<FailingMessage>
    {
        private FakeMessageChannel messageChannel;
        private Host host;
        private IBus bus;

        protected override void Given()
        {
            FakeMessageChannel.Reset();
            messageChannel = new FakeMessageChannel();
            bus = new CoreBus(messageChannel, new FakeMessageRouting(), new InMemorySubscriptionStorage());
            var config = new HostConfig();
            config.SetNumberOfRetrys(2);
            host = new Host(config);
            host.Start();
        }

        protected override void When()
        {
            bus.Send("Moon", new FailingMessage());
        }

        [Test]
        public void Message_should_be_placed_on_error_queue()
        {
            Assert.That(FakeMessageChannel.LastSentDestination.QueueName, Is.EqualTo("ezbus.core.error"));
        }

        public void Handle(FailingMessage message)
        {
            throw new Exception("Error in handler.");
        }
    }
}