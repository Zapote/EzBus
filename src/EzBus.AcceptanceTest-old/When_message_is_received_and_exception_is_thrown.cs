using System;
using EzBus.AcceptanceTest.Specifications;
using EzBus.AcceptanceTest.TestHelpers;
using NUnit.Framework;

namespace EzBus.AcceptanceTest
{
    [Specification]
    public class When_message_is_received_and_exception_is_thrown : BusSpecificationBase, IHandle<FailingMessage>
    {
        private static int retries;

        protected override void When()
        {
            bus.Send("Moon", new FailingMessage());
        }

        [Then]
        public void Message_should_be_retried_five_times()
        {
            Assert.That(retries, Is.EqualTo(5));
        }

        [Then]
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