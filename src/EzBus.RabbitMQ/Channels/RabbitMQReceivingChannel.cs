using System;
using System.IO;
using System.Text;
using EzBus.Config;
using RabbitMQ.Client.Events;

namespace EzBus.RabbitMQ.Channels
{
    public class RabbitMQReceivingChannel : RabbitMQChannel, IReceivingChannel
    {
        private readonly ISubscriptionCollection subscriptions;
        private EventingBasicConsumer consumer;

        public RabbitMQReceivingChannel(IChannelFactory channelFactory, ISubscriptionCollection subscriptions) : base(channelFactory)
        {
            if (subscriptions == null) throw new ArgumentNullException(nameof(subscriptions));
            this.subscriptions = subscriptions;
        }

        public void Initialize(EndpointAddress inputAddress, EndpointAddress errorAddress)
        {
            DeclareQueue(inputAddress.QueueName);
            DeclareQueue(errorAddress.QueueName);

            foreach (ISubscription subscription in subscriptions)
            {
                var exchange = subscription.Endpoint;
                channel.ExchangeDeclare(exchange, "fanout");
                channel.QueueBind(inputAddress.QueueName, exchange, string.Empty);
            }

            consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, eventArgs) =>
            {
                if (OnMessageReceived == null) return;

                var body = eventArgs.Body;
                var message = new ChannelMessage(new MemoryStream(body));

                foreach (var header in eventArgs.BasicProperties.Headers)
                {
                    var value = Encoding.UTF8.GetString((byte[])header.Value);
                    message.AddHeader(header.Key, value);
                }

                OnMessageReceived(this, new MessageReceivedEventArgs { Message = message });
            };

            channel.BasicConsume(inputAddress.QueueName, true, consumer);




        }

        public event EventHandler<MessageReceivedEventArgs> OnMessageReceived;
    }
}