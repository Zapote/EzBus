using System;
using EzBus.Core.ObjectFactory;
using EzBus.Core.Resolvers;
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
        private static int retries;

        protected override void Given()
        {
            var objectFactory = new DefaultObjectFactory();
            objectFactory.Initialize();

            messageChannel = new FakeMessageChannel();

            bus = new CoreBus(messageChannel, messageChannel, new FakeMessageRouting());
            host = new Host(new TaskRunner(objectFactory));
            host.Start();

            LogManager.SetLogLevel(LogLevel.Off);
        }

        protected override void When()
        {
            bus.Send("Moon", new FailingMessage());
        }

        [Test]
        public void Message_should_be_retried_five_times()
        {
            Assert.That(retries, Is.EqualTo(5));
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