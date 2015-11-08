using System;
using System.Collections.Generic;
using System.Linq;
using EzBus.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace EzBus.WindowsAzure.ServiceBus
{
    public class AzureTableSubscriptionStorage : IStartupTask
    {
        private static readonly ILogger log = LogManager.GetLogger(typeof(AzureTableSubscriptionStorage));
        private static CloudTable table;

        public void Store(string endpoint, Type messageType)
        {
            table.CreateIfNotExists();

            var type = messageType?.ToString() ?? string.Empty;
            var operation = TableOperation.InsertOrReplace(new SubscriptionEntity(endpoint, type));
            table.Execute(operation);
        }

        public IEnumerable<string> GetSubscribersEndpoints(string messageType)
        {
            try
            {
                var query = new TableQuery<SubscriptionEntity>();
                var result = table.ExecuteQuery(query);
                return result.Where(x =>
                    string.IsNullOrEmpty(x.MessageType) || x.MessageType == messageType)
                    .Select(x => x.Endpoint);
            }
            catch (Exception ex)
            {
                log.Error("Failed to get subscriptions", ex);
                return new List<string>();
            }
        }

        public void Run(IHostConfig config)
        {
            var cs = ConnectionStringHelper.GetStorageConnectionString();
            if (string.IsNullOrEmpty(cs)) return;

            var account = CloudStorageAccount.Parse(cs);
            var tableClient = account.CreateCloudTableClient();
            table = tableClient.GetTableReference(config.ErrorEndpointName.Replace(".", "-"));
        }
    }
}