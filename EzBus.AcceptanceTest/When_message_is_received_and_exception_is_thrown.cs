using System;
using System.Threading.Tasks;
using EzBus.AcceptanceTest.Specifications;
using EzBus.AcceptanceTest.TestHelpers;
using EzBus.Core;
using Xunit;

namespace EzBus.AcceptanceTest
{
    public class When_message_is_received_and_exception_is_thrown : IHandle<TestMessage>
    {
        private static int retries = 0;

        [Then]
        public async Task Message_should_be_retried_five_times()
        {
            retries = 0;
            var bus = BusFactory.Address("test")
                .UseTestBroker()
                .CreateBus();
            bus.Start().Wait();
            await bus.Send("test", new TestMessage { ThrowError = true });

            Assert.Equal(5, retries);
        }

        [Then]
        public void Message_should_be_placed_on_error_queue()
        {
            //Assert.Equal("testhost.error", FakeMessageChannel.LastSentDestination.Name);
        }

        public Task Handle(TestMessage m)
        {
            if (m.ThrowError)
            {
                retries++;
                throw new Exception("Cannot handle this message");
            }

            return Task.CompletedTask;
        }
    }
}