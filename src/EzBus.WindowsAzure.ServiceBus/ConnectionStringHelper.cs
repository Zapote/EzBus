using System;
using Microsoft.Azure;

namespace EzBus.WindowsAzure.ServiceBus
{
    public class ConnectionStringHelper
    {
        public static string GetServiceBusConnectionString()
        {
            const string defaultKey = "Microsoft.ServiceBus.ConnectionString";
            var machineKey = $"{defaultKey}.{Environment.MachineName}";
            var cs = CloudConfigurationManager.GetSetting(machineKey);

            if (string.IsNullOrEmpty(cs))
            {
                cs = CloudConfigurationManager.GetSetting(defaultKey);
            }

            return cs;
        }

        public static string GetStorageConnectionString()
        {
            const string defaultKey = "Microsoft.Storage.ConnectionString";
            var machineKey = $"{defaultKey}.{Environment.MachineName}";
            var cs = CloudConfigurationManager.GetSetting(machineKey);

            if (string.IsNullOrEmpty(cs))
            {
                cs = CloudConfigurationManager.GetSetting(defaultKey);
            }

            return cs;
        }

    }
}
