using EzBus.AcceptanceTest.Specifications;
using EzBus.AcceptanceTest.TestHelpers;
using EzBus.Core;
using Xunit;

namespace EzBus.AcceptanceTest
{
    public class When_message_is_received : IHandle<TestMessage>
    {
        private static bool messageHandled = false;

        public void Handle(TestMessage m)
        {
            messageHandled = true;
        }

        [Then]
        public void Message_is_handled()
        {
            messageHandled = false;

            var bus = BusFactory.Configure("test")
                .UseTestBroker()
                .CreateBus();
            
            bus.Start().Wait();
            bus.Send("test", new TestMessage()).Wait();

            Assert.True(messageHandled);
        }
    }
}