using System.Threading.Tasks;

namespace EzBus.Core
{
    public class StopConsumers : IShutdownTask
    {
        public string Name => "StopConsumers";

        public Task Run()
        {
            return Task.CompletedTask;
        }
    }
}