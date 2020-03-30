using EzBus.Core.Serializers;
using System;
using System.Threading.Tasks;

namespace EzBus.AcceptanceTest.TestHelpers
{
    public class TestConsumer : IConsumer
    {
        private readonly IAddressConfig config;
        Func<BasicMessage, Task> onMessage;

        public TestConsumer(IAddressConfig config)
        {
            this.config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public Task Consume(Func<BasicMessage, Task> onMessage)
        {
            this.onMessage = onMessage;
            return Task.CompletedTask;
        }

        public void Invoke(BasicMessage basicMessage)
        {
            var dest = basicMessage.GetHeader(MessageHeaders.Destination);
            if (dest != config.Address) return;
            onMessage.Invoke(basicMessage);
        }
    }
}