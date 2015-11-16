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
            CreateEndpointNames();
        }

        public int WorkerThreads { get; private set; }
        public int NumberOfRetrys { get; private set; }

        public string EndpointName { get; set; }
        public string ErrorEndpointName { get; set; }


        public void SetNumberOfWorkerThreads(int threads)
        {
            WorkerThreads = threads;
        }

        public void SetNumberOfRetrys(int value)
        {
            NumberOfRetrys = value;
        }
        
        private void CreateEndpointNames()
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            var assemblyName = this.GetAssemblyName();
            if (entryAssembly != null)
            {
                assemblyName = entryAssembly.GetName().Name;
            }
            EndpointName = assemblyName;
            ErrorEndpointName = $"{assemblyName}.error";
        }
    }
}