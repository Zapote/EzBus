using System.IO;
using EzBus.Config;
using Microsoft.Extensions.Configuration;

namespace EzBus.Core.Config
{
    public class EzBusConfig : IEzBusConfig
    {
        private EzBusConfig() { }

        public static IEzBusConfig GetConfig()
        {
            var currentDirectory = Directory.GetCurrentDirectory();

            var builder = new ConfigurationBuilder()
                .SetBasePath(currentDirectory)
                .AddJsonFile("ezbus.config.json", true, true);

            var cfg = builder.Build();
            var section = cfg.GetSection("ezbus");

            var busConfig = new EzBusConfig();
            section.Bind(busConfig);
            return busConfig;
        }

        public string EndpointName { get; set; }
        public Destination[] Destinations { get; set; }
        public Subscription[] Subscriptions { get; set; }
    }
}