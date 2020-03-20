using System.Threading.Tasks;

namespace EzBus.Core
{
    public class StopBroker : IShutdownTask
    {
        private readonly IBroker broker;

        public StopBroker(IBroker broker)
        {
            this.broker = broker ?? throw new System.ArgumentNullException(nameof(broker));
        }

        public string Name => "StopBroker";

        public Task Run()
        {
            return broker.Stop();
        }
    }
}