using EzBus.Serializers;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("EzBus.AcceptanceTest")]
namespace EzBus.Core
{
    internal sealed class Bus : IBus, ISubscriber, ISender, IPublisher
    {
        private readonly IMessageBroker broker;
        private readonly IConfig conf;
        private readonly IServiceProvider serviceProvider;
        private readonly IBodySerializer serializer = new Serializers.JsonBodySerializer();
        private readonly IHandlerCache handlerCache = new HandlerCache();

        public Bus(IMessageBroker broker, IConfig conf, IServiceProvider serviceProvider)
        {
            this.broker = broker ?? throw new System.ArgumentNullException(nameof(broker));
            this.conf = conf ?? throw new System.ArgumentNullException(nameof(conf));
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public async Task Start()
        {
            handlerCache.Prime();
            await broker.Start(conf.Address, conf.ErrorAddress);
            var consumer = await broker.CreateConsumer();
            await consumer.Consume(OnMessage);
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

        private void OnMessage(BasicMessage basicMessage)
        {
            var messageTypeName = basicMessage.GetHeader(MessageHeaders.MessageFullname);
            var handlerInfos = handlerCache.GetHandlerInfo(messageTypeName);

            var messageType = handlerInfos.First().MessageType;
            var message = serializer.Deserialize(basicMessage.BodyStream, messageType);

            foreach (var info in handlerInfos)
            {
                var handlerType = info.HandlerType;

                //log.Verbose($"Invoking handler {handlerType.Name}");

                var result = InvokeHandler(handlerType, message);

                if (!result.Success)
                {
                    throw result.Exception;
                }
            }
        }

        private InvokationResult InvokeHandler(Type handlerType, object message)
        {
            var success = true;
            Exception exception = null;

            for (var i = 0; i < conf.NumberOfRetries; i++)
            {
                var methodInfo = handlerType.GetMethod("Handle", new[] { message.GetType() });

                try
                {
                    var handler = serviceProvider.GetService(handlerType);
                    methodInfo.Invoke(handler, new[] { message });
                    success = true;
                    break;
                }
                catch (Exception ex)
                {
                    //log.Error(string.Format("Attempt {1}: Failed to handle message '{0}'.", message.GetType().Name, i + 1), ex.InnerException);
                    success = false;
                    exception = ex.InnerException;
                }
            }

            return new InvokationResult(success, exception);
        }
    }
}
