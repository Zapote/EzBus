using EzBus.AcceptanceTest.Specifications;
using EzBus.AcceptanceTest.TestHelpers;
using Xunit;

namespace EzBus.AcceptanceTest
{
    public class When_message_is_received : BusSpecificationBase
    {
        public When_message_is_received()
        {
            bus.Send("Moon", new TestMessage());
        }

        [Then]
        public void Message_is_handled()
        {
            Assert.True(TestMessageHandler.MessageIsHandled);
        }
    }

    public class TestMessageHandler : IHandle<TestMessage>
    {
        public static bool MessageIsHandled { get; private set; }

        public void Handle(TestMessage message)
        {
            MessageIsHandled = true;
        }
    }
}