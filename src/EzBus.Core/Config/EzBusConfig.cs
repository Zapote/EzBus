using System.Collections.Generic;
using System.IO;
using System.Linq;
using EzBus.Config;
using Microsoft.Extensions.Configuration;

namespace EzBus.Core.Config
{
    public class EzBusConfig : IEzBusConfig
    {
        private readonly IDictionary<string, string> connectionStrings = new Dictionary<string, string>();
        private static IConfigurationRoot configurationRoot;
        private EzBusConfig() { }

        public static IEzBusConfig GetConfig()
        {
            var currentDirectory = Directory.GetCurrentDirectory();

            var builder = new ConfigurationBuilder()
                .SetBasePath(currentDirectory)
                .AddJsonFile("ezbus.config.json", true, true);

            configurationRoot = builder.Build();
            var root = configurationRoot.GetSection("ezbus");

            var busConfig = new EzBusConfig
            {
                EndpointName = root["endpointName"],
                Destinations = ResolveDestionations(),
                Subscriptions = ResolveSubscriptions()
            };

            AddConnectionStrings(busConfig);

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

        private static Destination[] ResolveDestionations()
        {
            var section = configurationRoot.GetSection("ezbus:destinations");
            return section
                .GetChildren()
                .Select(item => new Destination
                {
                    Endpoint = item["endpoint"],
                    Namespace = item["namespace"],
                    Message = item["message"]
                }).ToArray();
        }

        private static Subscription[] ResolveSubscriptions()
        {
            var section = configurationRoot.GetSection("ezbus:subscriptions");
            return section
                .GetChildren()
                .Select(item => new Subscription { Endpoint = item["endpoint"] }).ToArray();
        }

        private static void AddConnectionStrings(EzBusConfig busConfig)
        {
            var connectionstringsSection = configurationRoot.GetSection("ezbus:connectionstrings");
            foreach (var item in connectionstringsSection.GetChildren())
            {
                busConfig.AddConnectionString(item.Key, item.Value);
            }
        }
    }
}