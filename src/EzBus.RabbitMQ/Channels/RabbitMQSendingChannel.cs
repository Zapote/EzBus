using RabbitMQ.Client;

namespace EzBus.RabbitMQ.Channels
{
    public class RabbitMQSendingChannel : RabbitMQChannel, ISendingChannel
    {
        private readonly IModel channel;

        public RabbitMQSendingChannel(IChannelFactory channelFactory)
            : base(channelFactory)
        {
            channel = channelFactory.GetChannel();
        }

        public void Send(EndpointAddress destination, ChannelMessage channelMessage)
        {
            DeclareQueuePassive(destination.QueueName);
            var properties = ConstructHeaders(channelMessage);
            var body = channelMessage.BodyStream.ToByteArray();
            channel.BasicPublish(string.Empty,
                                 destination.QueueName,
                                 basicProperties: properties,
                                 body: body,
                                 mandatory: true);
        }
    }
}

