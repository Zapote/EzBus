using System;
using EzBus.Core.ObjectFactory;
using EzBus.Core.Test.TestHelpers;
using EzBus.Logging;
using NUnit.Framework;

namespace EzBus.Core.Test.Specifications
{
    [Specification]
    public class When_message_is_received : SpecificationBase, IHandle<TestMessage>
    {
        private FakeMessageChannel messageChannel;
        private Host host;
        private IBus bus;
        private static bool messageIsHandled;

        protected override void Given()
        {
            var objectFactory = new DefaultObjectFactory();
            objectFactory.Initialize();

            messageChannel = new FakeMessageChannel();

            bus = new CoreBus(messageChannel, messageChannel, new FakeMessageRouting());
            host = new Host(new HostConfig(), objectFactory);
            host.Start();

            LogManager.SetLogLevel(LogLevel.Off);
        }

        protected override void When()
        {
            bus.Send("Moon", new TestMessage());
        }

        [Then]
        public void Message_is_handled()
        {
            Assert.That(messageIsHandled, Is.True);
        }

        public void Handle(TestMessage message)
        {
            messageIsHandled = true;
        }
    }
}