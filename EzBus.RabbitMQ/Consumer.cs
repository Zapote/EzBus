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
        private IChannel channel;
        private Func<BasicMessage, Task> onMessage;

        public Consumer(IChannelFactory factory, string queue)
        {
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
            this.queue = queue;
        }

        public async Task Consume(Func<BasicMessage, Task> onMessage)
        {
            this.onMessage = onMessage;
            channel = await factory.GetChannel();
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += OnReceivedMessage;
            await channel.BasicConsumeAsync(queue, false, string.Empty, false, false, null, consumer);
        }

        private async Task OnReceivedMessage(object sender, BasicDeliverEventArgs args)
        {
            var body = args.Body.ToArray();
            var message = new BasicMessage(new MemoryStream(body));

            foreach (var header in args.BasicProperties.Headers)
            {
                var value = Encoding.UTF8.GetString((byte[])header.Value);
                message.AddHeader(header.Key, value);
            }

            await onMessage(message);
            await channel.BasicAckAsync(args.DeliveryTag, false);
        }
    }
}
