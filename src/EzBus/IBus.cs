using System.Threading.Tasks;

namespace EzBus
{
    public interface IBus
    {
        void Send(object message);
        void Send(string destinationQueue, object message);
        void Publish(object message);
    }
}
