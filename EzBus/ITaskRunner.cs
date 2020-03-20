using System.Threading.Tasks;

namespace EzBus
{
    public interface ITaskRunner
    {
        Task Run<T>() where T : ITask;
    }
}