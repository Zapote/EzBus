using EzBus.Core.Serializers;
using EzBus.Serializers;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("EzBus.AcceptanceTest")]
namespace EzBus.Core
{
    internal sealed class Bus : IBus, ISubscriber, ISender, IPublisher
    {
        private readonly IBroker broker;
        private readonly ITaskRunner taskRunner;
        private readonly IBodySerializer serializer = new JsonBodySerializer();

        public Bus(IBroker broker, ITaskRunner taskRunner)
        {
            this.broker = broker;
            this.taskRunner = taskRunner;
        }

        public async Task Start()
        {
            await taskRunner.Run<IStartupTask>();
        }

        public async Task Stop()
        {
            await taskRunner.Run<IShutdownTask>();
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

        public Task Subscribe(string endpoint, string messageName)
        {
            throw new System.NotImplementedException();
        }

        public Task Unsubscribe(string endpoint, string messageName = null)
        {
            throw new System.NotImplementedException();
        }
    }
}
