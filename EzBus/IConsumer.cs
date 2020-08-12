using System;
using System.Threading.Tasks;

namespace EzBus
{
    public interface IConsumer
    {
        Task Consume(Func<BasicMessage, Task> onMessage);
    }
}
