using System.Threading.Tasks;

namespace EzBus
{
    public interface IHandle<in T> : IMessageHandler
    {
        Task Handle(T m);
    }

    public interface IMessageHandler
    {

    }
}