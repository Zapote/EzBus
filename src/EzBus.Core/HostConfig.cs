using System.Reflection;
using EzBus.Core.Utils;

namespace EzBus.Core
{
    public class HostConfig : IHostConfig
    {
        public HostConfig()
        {
            NumberOfRetrys = 5;
            WorkerThreads = 1;
            EndpointName = CreateEndpointName();
        }

        public int WorkerThreads { get; private set; }
        public int NumberOfRetrys { get; private set; }

        public string EndpointName { get; private set; }

        public void SetNumberOfWorkerThreads(int threads)
        {
            WorkerThreads = threads;
        }

        public void SetNumberOfRetrys(int value)
        {
            NumberOfRetrys = value;
        }

        private string CreateEndpointName()
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly == null) return this.GetAssemblyName();
            return entryAssembly.GetName().Name;
        }
    }
}