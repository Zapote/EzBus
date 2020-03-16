using EzBus.Utils;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EzBus.RabbitMQ
{
    public class Broker : IMessageBroker
    {
        private readonly IChannelFactory channelFactory;
        private IModel channel;

        public Broker(IChannelFactory channelFactory)
        {
            this.channelFactory = channelFactory ?? throw new ArgumentNullException(nameof(channelFactory));
        }

        public Task Publish(BasicMessage message)
        {
            throw new NotImplementedException();
        }

        public Task Send(string destination, BasicMessage message)
        {
            QueueDeclarePassive(destination);
            var properties = ConstructHeaders(message);
            var body = message.BodyStream.ToByteArray();

            lock (channel)
            {
                channel.BasicPublish(string.Empty,
                    destination,
                    basicProperties: properties,
                    body: body,
                    mandatory: true);
            }

            return Task.CompletedTask;
        }

        public Task Start()
        {
            channel = channelFactory.GetChannel();
            return Task.CompletedTask;
        }

        public Task Stop()
        {
            throw new NotImplementedException();
        }

        private IBasicProperties ConstructHeaders(BasicMessage message)
        {
            var props = channel.CreateBasicProperties();

            props.ClearHeaders();
            props.Persistent = true;
            props.Headers = new Dictionary<string, object>();
            foreach (var h in message.Headers)
            {
                props.Headers.Add(h.Name, h.Value);
            }

            return props;
        }

        protected void QueueDeclarePassive(string queueName)
        {
            try
            {
                channel.QueueDeclarePassive(queueName);
            }
            catch (OperationInterruptedException ex)
            {
                if (ex.ShutdownReason.ReplyCode != 404) throw;

                var message = $"Queue '{queueName}' does not exist or is currently not available.";
                throw new InvalidOperationException(message, ex);
            }
        }
    }
}
