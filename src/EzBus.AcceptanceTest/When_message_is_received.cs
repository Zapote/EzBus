using EzBus.AcceptanceTest.Specifications;
using EzBus.AcceptanceTest.TestHelpers;
using EzBus.Core;
using EzBus.Core.ObjectFactory;
using EzBus.Logging;
using NUnit.Framework;

namespace EzBus.AcceptanceTest
{
    [Specification]
    public class When_message_is_received : BusSpecificationBase, IHandle<TestMessage>
    {
        private static bool handled;

        protected override void When()
        {
            bus.Send("Moon", new TestMessage());
        }

        [Then]
        public void Message_is_handled()
        {
            Assert.That(handled, Is.True);
        }

        public void Handle(TestMessage message)
        {
            handled = true;
        }
    }
}