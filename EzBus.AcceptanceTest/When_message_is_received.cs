using EzBus.AcceptanceTest.Specifications;
using EzBus.AcceptanceTest.TestHelpers;
using EzBus.Core;
using System.Threading.Tasks;
using Xunit;

namespace EzBus.AcceptanceTest
{
    public class When_message_is_received : IHandle<TestMessage>
    {
        private static bool messageHandled = false;

        public Task Handle(TestMessage m)
        {
            messageHandled = true;
            return Task.CompletedTask;
        }

        [Then]
        public void Message_is_handled()
        {
            messageHandled = false;

            var bus = BusFactory.Address("test")
                .UseTestBroker()
                .CreateBus();

            bus.Start().Wait();
            bus.Send("test", new TestMessage()).Wait();

            Assert.True(messageHandled);
        }
    }
}