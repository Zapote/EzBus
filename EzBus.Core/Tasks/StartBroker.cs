using System.Threading.Tasks;

namespace EzBus.Core
{
    public class StartBroker : ISystemStartupTask
    {
        private readonly IBroker broker;

        public string Name => "StartBroker";

        public int Prio => 200;

        public StartBroker(IBroker broker)
        {
            this.broker = broker;
        }

        public Task Run()
        {
            return broker.Start();
        }
    }
}