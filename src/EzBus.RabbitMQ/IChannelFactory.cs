using RabbitMQ.Client;

namespace EzBus.RabbitMQ
{
    public interface IChannelFactory
    {
        IModel GetChannel();
    }
}