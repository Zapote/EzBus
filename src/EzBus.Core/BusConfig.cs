using System.IO;
using EzBus.Logging;
using EzBus.Utils;
using Microsoft.Extensions.PlatformAbstractions;

namespace EzBus.Core
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
        public int NumberOfRetries { get; set; } = 5;
        public int WorkerThreads { get; set; } = 1;

        private void CreateEndpointNames()
        {
            var applicationEnvironment = PlatformServices.Default.Application;
            var applicationName = applicationEnvironment.ApplicationName;

            if (applicationName.IsNullOrEmpty())
            {
                applicationName = Path.GetPathRoot(applicationEnvironment.ApplicationBasePath);
            }

            EndpointName = applicationName;
            ErrorEndpointName = $"{applicationName}.error";
        }
    }
}
