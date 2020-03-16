using System.Threading.Tasks;

namespace EzBus
{
    public interface IBus : ISender, IPublisher
    {
        Task Start();
        Task Stop();
    }

    public interface ISubscriber
    {
        Task Subscribe(string endpoint, string messageName);
        Task Unsubscribe(string endpoint, string messageName = null);
    }

    public interface ISender
    {
        Task Send(string destination, object message);
    }

    public interface IPublisher
    {
        Task Publish(object message);
    }
}