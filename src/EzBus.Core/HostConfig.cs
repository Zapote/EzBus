using EzBus.Core.Builders;

namespace EzBus.Core
{
    public class HostConfig
    {
        public IObjectFactory ObjectFactory { get; private set; }
        public int WorkerThreads { get; private set; }
        public int NumberOfRetrys { get; private set; }

        public HostConfig()
        {
            NumberOfRetrys = 5;
            WorkerThreads = 1;
            ObjectFactory = new LightInjectObjectFactory();
        }

        public void SetNumberOfWorkerThreads(int threads)
        {
            WorkerThreads = threads;
        }

        public void SetNumberOfRetrys(int value)
        {
            NumberOfRetrys = value;
        }
    }
}