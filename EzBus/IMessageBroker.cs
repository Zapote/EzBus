using System.Threading.Tasks;

namespace EzBus
{
    public interface IMessageBroker
    {
        Task Start();
        Task Stop();
        Task Send(string destination, BasicMessage message);
        Task Publish(BasicMessage message);
    }
}