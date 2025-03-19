using System;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace EzBus.RabbitMQ
{
    public interface IChannelFactory
    {
        Task<IChannel> GetChannel();
        Task Close();
    }
}
