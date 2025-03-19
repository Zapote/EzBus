using EzBus.Utils;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EzBus.RabbitMQ
{
    public class Broker : IBroker
    {
        private readonly IChannelFactory channelFactory;
        private readonly IConfig conf;
        private readonly string address;
        private readonly string errorAddress;
        private IChannel channel;

        public Broker(IChannelFactory channelFactory, IConfig conf, IAddressConfig addressConf)
        {
            this.channelFactory = channelFactory ?? throw new ArgumentNullException(nameof(channelFactory));
            this.conf = conf ?? throw new ArgumentNullException(nameof(conf));

            address = addressConf.Address;
            errorAddress = addressConf.ErrorAddress;
            channel = channelFactory.GetChannel().Result;
        }

        public async Task Publish(BasicMessage message)
        {
            var exchange = address;
            var properties = ConstructHeaders(message);
            var body = message.BodyStream.ToByteArray();
            var messageName = message.GetHeader(MessageHeaders.MessageName);

            await channel.BasicPublishAsync(exchange, messageName, true, properties, body);
        }

        public async Task Send(string destination, BasicMessage message)
        {
            var properties = ConstructHeaders(message);
            var body = message.BodyStream.ToByteArray();

            await QueueDeclarePassive(destination);
            await channel.BasicPublishAsync(string.Empty, destination, true, properties, body);
        }

        public async Task Start()
        {
            await channel.QueueDeclareAsync(address, true, false, false);
            await channel.QueueDeclareAsync(errorAddress, true, false, false);
            await channel.ExchangeDeclareAsync(address, conf.ExchangeType, true);
        }

        public Task<IConsumer> CreateConsumer()
        {
            return Task.FromResult<IConsumer>(new Consumer(channelFactory, address));
        }

        public async Task Stop()
        {
            await channelFactory.Close();
        }

        private static BasicProperties ConstructHeaders(BasicMessage message)
        {
            var props = new BasicProperties();
            props.ClearHeaders();
            props.Persistent = true;
            props.Headers = new Dictionary<string, object>();

            foreach (var h in message.Headers)
            {
                props.Headers.Add(h.Name, h.Value);
            }

            return props;
        }

        protected async Task QueueDeclarePassive(string queueName)
        {
            try
            {
                await channel.QueueDeclarePassiveAsync(queueName);
            }
            catch (OperationInterruptedException ex)
            {
                if (ex.ShutdownReason.ReplyCode != 404) throw;
                await RestoreChannel();
                var message = $"Queue '{queueName}' does not exist or is currently not available.";
                throw new InvalidOperationException(message, ex);
            }
        }

        private async Task RestoreChannel()
        {
            channel = await channelFactory.GetChannel();
        }
    }
}
