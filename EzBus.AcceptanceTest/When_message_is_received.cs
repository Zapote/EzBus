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
        public async Task Message_is_handled()
        {
            messageHandled = false;

            var bus = BusFactory.Address("test")
                .UseTestBroker()
                .CreateBus();

            await bus.Start();
            await bus.Send("test", new TestMessage());

            Assert.True(messageHandled);
        }
    }
}