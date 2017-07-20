using System;
using EzBus.AcceptanceTest.Specifications;
using EzBus.AcceptanceTest.TestHelpers;
using Xunit;

namespace EzBus.AcceptanceTest
{
    public class When_message_is_received_and_exception_is_thrown : BusSpecificationBase
    {
        public When_message_is_received_and_exception_is_thrown()
        {
            bus.Send("Moon", new FailingMessage());
        }

        [Then]
        public void Message_should_be_retried_five_times()
        {
            Assert.Equal(5, FailingMessageHandler.Retries);
        }

        [Then]
        public void Message_should_be_placed_on_error_queue()
        {
            Assert.Equal("testhost.error", FakeMessageChannel.LastSentDestination.QueueName);
        }
    }

    public class FailingMessageHandler : IHandle<FailingMessage>
    {
        public static int Retries { get; private set; }

        public void Handle(FailingMessage message)
        {
            Retries++;
            throw new Exception("Testing error in handler.");
        }
    }
}