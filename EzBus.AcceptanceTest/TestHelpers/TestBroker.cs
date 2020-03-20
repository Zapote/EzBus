using EzBus.Serializers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace EzBus.AcceptanceTest.TestHelpers
{
    public class TestBroker : IBroker
    {
        private readonly IServiceProvider serviceProvider;

        public TestBroker(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public Task Publish(BasicMessage message)
        {
            return Task.CompletedTask;
        }

        public Task Send(string destination, BasicMessage basicMessage)
        {
            var consumers = serviceProvider.GetServices<IConsumer>();
            basicMessage.AddHeader(MessageHeaders.Destination, destination);
            
            foreach (var consumer in consumers)
            {
                ((TestConsumer)consumer).Invoke(basicMessage);
            }

            return Task.CompletedTask;
        }

        public Task Start()
        {
            return Task.CompletedTask;
        }

        public Task Stop()
        {
            return Task.CompletedTask;
        }
    }
}