using System.Threading.Tasks;

namespace EzBus.Core
{
    public interface IHandlerInvoker
    {
        Task Invoke(BasicMessage basicMessage);
    }
}