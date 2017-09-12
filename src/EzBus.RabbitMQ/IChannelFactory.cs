using System;
using RabbitMQ.Client;

namespace EzBus.RabbitMQ
{
    [CLSCompliant(false)]
    public interface IChannelFactory
    {
        IModel GetChannel();
    }
}