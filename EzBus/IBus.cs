using System.Threading.Tasks;

namespace EzBus
{
  public interface IBus : ISender, IPublisher, ISubscriber
  {
    Task Start();
    Task Stop();
  }

  public interface ISubscriber
  {
    Task Subscribe(string address, string messageName);
    Task Unsubscribe(string address, string messageName);
  }

  public interface ISender
  {
    Task Send(string destination, object message);
  }

  public interface IPublisher
  {
    Task Publish(object message);
  }

  public interface ISubscriptionManager
  {
    Task Subscribe(string address, string messageName);
    Task Unsubscribe(string address, string messageName);
  }
}