using EzBus.Serializers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("EzBus.AcceptanceTest")]
namespace EzBus.Core
{
    internal sealed class Bus : IBus, ISubscriber, ISender, IPublisher
    {
        private readonly IMessageBroker broker;
        private readonly IBodySerializer serializer = new Serializers.JsonBodySerializer();

        public Bus(IMessageBroker broker)
        {
            this.broker = broker ?? throw new System.ArgumentNullException(nameof(broker));
        }

        public Task Start()
        {
            return broker.Start();
        }

        public Task Stop()
        {
            return Task.CompletedTask;
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
