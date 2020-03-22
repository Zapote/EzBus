using EzBus.Core.Serializers;
using EzBus.Serializers;
using Microsoft.Extensions.Logging;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("EzBus.AcceptanceTest")]
namespace EzBus.Core
{
    internal sealed class Bus : IBus
    {
        private readonly IBroker broker;
        private readonly ITaskRunner taskRunner;
        private readonly ISubscriptionManager subscriptionManager;
        private readonly ILogger<Bus> logger;
        private readonly IBodySerializer serializer = new JsonBodySerializer();

        public Bus(IBroker broker, ITaskRunner taskRunner, ISubscriptionManager subscriptionManager, ILogger<Bus> logger)
        {
            this.broker = broker ?? throw new ArgumentNullException(nameof(broker));
            this.taskRunner = taskRunner ?? throw new ArgumentNullException(nameof(taskRunner));
            this.subscriptionManager = subscriptionManager ?? throw new ArgumentNullException(nameof(subscriptionManager));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Start()
        {
            logger.LogInformation("Starting EzBus");
            await taskRunner.Run<IStartupTask>();
            logger.LogInformation("EzBus started");
        }

        public async Task Stop()
        {
            logger.LogInformation("Stopping EzBus");
            await taskRunner.Run<IShutdownTask>();
            logger.LogInformation("EzBus stopped");
        }

        public Task Publish(object message)
        {
            var m = MessageFactory.Create(message, serializer);
            return broker.Publish(m);
        }

        public Task Send(string destination, object message)
        {
            var m = MessageFactory.Create(message, serializer);
            m.AddHeader(MessageHeaders.Destination, destination);
            return broker.Send(destination, m);
        }

        public Task Subscribe(string address, string messageName)
        {
            return subscriptionManager.Subscribe(address, messageName);
        }

        public Task Unsubscribe(string address, string messageName = null)
        {
            return subscriptionManager.Unsubscribe(address, messageName);
        }
    }
}
