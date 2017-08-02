using System.Reflection;
using EzBus.Logging;
using EzBus.Utils;

namespace EzBus
{
    public class BusConfig : IBusConfig
    {
        public BusConfig()
        {
            CreateEndpointNames();
        }

        public string EndpointName { get; set; }
        public string ErrorEndpointName { get; set; }
        public LogLevel LogLevel { get; set; } = LogLevel.Info;
        public int NumberOfRetrys { get; set; } = 5;
        public int WorkerThreads { get; set; } = 1;

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