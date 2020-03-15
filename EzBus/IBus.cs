using System.Threading.Tasks;

namespace EzBus
{
    public interface IBus
    {
        Task Send(string endpoint, object message);
        Task Publish(object message);
    }
}
