namespace EzBus.Core
{
    public class HostConfig
    {
        public int WorkerThreads { get; private set; }
        public int NumberOfRetrys { get; private set; }
        
        public HostConfig()
        {
            NumberOfRetrys = 5;
            WorkerThreads = 1;
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