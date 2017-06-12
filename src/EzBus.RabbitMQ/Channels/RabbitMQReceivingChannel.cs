using System;
using System.IO;
using System.Text;
using System.Threading;
using EzBus.Config;
using EzBus.Logging;
using RabbitMQ.Client.Events;

namespace EzBus.RabbitMQ.Channels
{
    public class RabbitMQReceivingChannel : RabbitMQChannel, IReceivingChannel
    {
        private static readonly ILogger log = LogManager.GetLogger<RabbitMQReceivingChannel>();
        private readonly IEzBusConfig config;
        private EventingBasicConsumer consumer;

        public RabbitMQReceivingChannel(IChannelFactory channelFactory, IEzBusConfig config) : base(channelFactory)
        {
            this.config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public void Initialize(EndpointAddress inputAddress, EndpointAddress errorAddress)
        {
            DeclareQueue(inputAddress.QueueName);
            DeclareQueue(errorAddress.QueueName);
            DeclareExchange(inputAddress.QueueName);

            BindSubscriptionExchanges(inputAddress);

            consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                if (OnMessage == null)
                {
                    channel.BasicAck(ea.DeliveryTag, false);
                    return;
                }

                var body = ea.Body;
                var message = new ChannelMessage(new MemoryStream(body));

                foreach (var header in ea.BasicProperties.Headers)
                {
                    var value = Encoding.UTF8.GetString((byte[])header.Value);
                    message.AddHeader(header.Key, value);
                }

                OnMessage(message);

                channel.BasicAck(ea.DeliveryTag, false);
            };

            channel.BasicConsume(inputAddress.QueueName, false, string.Empty, false, false, null, consumer);
        }

        public Action<ChannelMessage> OnMessage { get; set; }

        private void BindSubscriptionExchanges(EndpointAddress inputAddress)
        {
            if (config.Subscriptions == null)
            {
                log.Info("No subscriptions found in config");
                return;
            }

            foreach (var subscription in config.Subscriptions)
            {
                var exchange = subscription.Endpoint;
                BindQueue(inputAddress.QueueName, exchange);
            }
        }
    }
}