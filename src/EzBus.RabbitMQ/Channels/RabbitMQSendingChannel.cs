namespace EzBus.RabbitMQ.Channels
{
    public class RabbitMQSendingChannel : RabbitMQChannel, ISendingChannel
    {
        public void Send(EndpointAddress destination, ChannelMessage channelMessage)
        {
            DeclareQueuePassive(destination.QueueName);
            var properties = ConstructHeaders(channelMessage);
            var body = channelMessage.BodyStream.ToByteArray();
            channel.BasicPublish(exchange: "",
                                 routingKey: destination.QueueName,
                                 basicProperties: properties,
                                 body: body,
                                 mandatory: true);
        }
    }
}

