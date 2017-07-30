using EzBus.AcceptanceTest.Specifications;
using EzBus.AcceptanceTest.TestHelpers;
using Xunit;

namespace EzBus.AcceptanceTest
{
    public class When_message_is_received : BusSpecificationBase
    {
        public When_message_is_received()
        {
            bus.Send("Moon", new ToBeReceivedMessage());
        }

        [Then]
        public void Message_is_handled()
        {
            Assert.True(ToBeReceivedMessageMessageHandler.MessageIsHandled);
        }
    }

    public class ToBeReceivedMessageMessageHandler : IHandle<ToBeReceivedMessage>
    {
        public static bool MessageIsHandled { get; private set; }

        public void Handle(ToBeReceivedMessage message)
        {
            MessageIsHandled = true;
        }
    }
}