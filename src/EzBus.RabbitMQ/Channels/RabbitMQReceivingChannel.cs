using System;
using System.IO;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace EzBus.RabbitMQ.Channels
{
    public class RabbitMQReceivingChannel : RabbitMQChannel, IReceivingChannel
    {
        private EventingBasicConsumer consumer;

        public RabbitMQReceivingChannel(IChannelFactory channelFactory) : base(channelFactory)
        {
        }

        public void Initialize(EndpointAddress inputAddress, EndpointAddress errorAddress)
        {
            DeclareQueue(inputAddress.QueueName);
            DeclareQueue(errorAddress.QueueName);

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

            channel.BasicConsume(queue: inputAddress.QueueName,
                                 noAck: true,
                                 consumer: consumer);

        }

        public event EventHandler<MessageReceivedEventArgs> OnMessageReceived;


    }
}