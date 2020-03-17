using System;
using System.Threading.Tasks;

namespace EzBus
{
    public interface IMessageBroker
    {
        Task Start(string address, string errorAddress);
        Task Stop();
        Task Send(string destination, BasicMessage message);
        Task Publish(BasicMessage message);
        Task<IConsumer> CreateConsumer();
    }

    public interface IConsumer
    {
        Task Consume(Action<BasicMessage> onMessage);
    }
}
