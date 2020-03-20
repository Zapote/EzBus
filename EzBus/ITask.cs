using System.Threading.Tasks;

namespace EzBus
{
    public interface ITask
    {
        string Name { get; }
        Task Run();
    }
}
