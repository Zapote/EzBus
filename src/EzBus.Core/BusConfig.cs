using System.Reflection;
using EzBus.Core.Utils;

namespace EzBus.Core
{
    public class BusConfig : IBusConfig
    {
        public BusConfig()
        {
            NumberOfRetrys = 5;
            WorkerThreads = 1;
            CreateEndpointNames();
        }

        public int WorkerThreads { get; set; }
        public int NumberOfRetrys { get; set; }
        public string EndpointName { get; set; }
        public string ErrorEndpointName { get; set; }

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