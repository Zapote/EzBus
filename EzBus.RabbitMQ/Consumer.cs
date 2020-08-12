using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace EzBus.RabbitMQ
{
    internal class Consumer : IConsumer
    {
        private readonly IChannelFactory factory;
        private readonly string queue;
        private IModel channel;
        private Func<BasicMessage, Task> onMessage;

        public Consumer(IChannelFactory factory, string queue)
        {
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
            this.queue = queue;
        }

        public Task Consume(Func<BasicMessage, Task> onMessage)
        {
            this.onMessage = onMessage;
            channel = factory.GetChannel();
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += OnReceivedMessage;
            channel.BasicConsume(queue, false, string.Empty, false, false, null, consumer);
            return Task.CompletedTask;
        }

        private void OnReceivedMessage(object sender, BasicDeliverEventArgs args)
        {
            var body = args.Body;
            var message = new BasicMessage(new MemoryStream(body));

            foreach (var header in args.BasicProperties.Headers)
            {
                var value = Encoding.UTF8.GetString((byte[])header.Value);
                message.AddHeader(header.Key, value);
            }

            onMessage(message).GetAwaiter().GetResult();

            channel.BasicAck(args.DeliveryTag, false);
        }
    }
}
