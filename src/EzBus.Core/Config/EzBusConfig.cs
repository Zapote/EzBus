using System.Collections.Generic;
using System.IO;
using EzBus.Config;
using Microsoft.Extensions.Configuration;

namespace EzBus.Core.Config
{
    public class EzBusConfig : IEzBusConfig
    {
        private readonly IDictionary<string, string> connectionStrings = new Dictionary<string, string>();
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

            var cs = section.GetSection("connectionstrings");
            foreach (var item in cs.GetChildren())
            {
                busConfig.AddConnectionString(item.Key, item.Value);
            }

            return busConfig;
        }

        public string EndpointName { get; set; }
        public Destination[] Destinations { get; set; }
        public Subscription[] Subscriptions { get; set; }

        public string GetConnectionString(string name)
        {
            return !connectionStrings.ContainsKey(name) ? null : connectionStrings[name];
        }

        private void AddConnectionString(string name, string value)
        {
            connectionStrings.Add(name, value);
        }
    }
}