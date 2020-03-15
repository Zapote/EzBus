using System.IO;
using EzBus.Logging;
using EzBus.Utils;
using Microsoft.Extensions.PlatformAbstractions;

namespace EzBus.Core
{
    public class BusConfig : IBusConfig
    {
        private string endpointName;

        public BusConfig()
        {
            CreateEndpointNames();
        }

        public string EndpointName
        {
            get => endpointName;
            set
            {
                endpointName = value;
                ErrorEndpointName = $"{endpointName}.error";
            }
        }

        public string ErrorEndpointName { get; set; }
        public LogLevel LogLevel { get; set; } = LogLevel.Info;
        public int NumberOfRetries { get; set; } = 5;
        public int WorkerThreads { get; set; } = 1;

        private void CreateEndpointNames()
        {
            var app = PlatformServices.Default.Application;
            var appName = app.ApplicationName;

            if (appName.IsNullOrEmpty())
            {
                appName = new DirectoryInfo(app.ApplicationBasePath).Name;
            }

            EndpointName = appName;
            ErrorEndpointName = $"{appName}.error";
        }
    }
}
