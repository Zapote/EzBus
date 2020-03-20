using System;
using System.Threading.Tasks;

namespace EzBus
{
    public interface IConsumer
    {
        Task Consume(Action<BasicMessage> onMessage);
    }
}
