using EzBus.Utils;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EzBus.RabbitMQ
{
    public class Broker : IBroker
    {
        private readonly IChannelFactory channelFactory;
        private readonly IConfig conf;
        private readonly string address;
        private readonly string errorAddress;
        private readonly IModel channel;

        public Broker(IChannelFactory channelFactory, IConfig conf, IAddressConfig addressConf)
        {
            this.channelFactory = channelFactory ?? throw new ArgumentNullException(nameof(channelFactory));
            this.conf = conf ?? throw new ArgumentNullException(nameof(conf));
            
            address = addressConf.Address;
            errorAddress = addressConf.ErrorAddress;

            channel = channelFactory.GetChannel();
        }

        public Task Publish(BasicMessage message)
        {
            var exchange = address;
            var properties = ConstructHeaders(message);
            var body = message.BodyStream.ToByteArray();
            var messageName = message.GetHeader(MessageHeaders.MessageName);

            lock (channel)
            {
                channel.BasicPublish(exchange, messageName, properties, body);
            }

            return Task.CompletedTask;
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
            channel.QueueDeclare(address, true, false, false);
            channel.QueueDeclare(errorAddress, true, false, false);
            channel.ExchangeDeclare(address, conf.ExchangeType, true);

            return Task.CompletedTask;
        }

        public Task<IConsumer> CreateConsumer()
        {
            return Task.FromResult<IConsumer>(new Consumer(channelFactory, address));
        }

        public Task Stop()
        {
            channelFactory.Close();
            return Task.CompletedTask;
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
