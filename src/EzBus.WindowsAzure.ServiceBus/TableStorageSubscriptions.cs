using System;
using System.Collections.Generic;
using System.Linq;
using EzBus.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace EzBus.WindowsAzure.ServiceBus
{
    public class AzureTableSubscriptionstorage : ISubscriptionStorage
    {
        private static readonly ILogger log = LogManager.GetLogger(typeof(AzureTableSubscriptionstorage));
        private CloudTable table;
        private bool tableCreated;

        public void Initialize(string endpointName)
        {
            var cs = ConnectionStringHelper.GetStorageConnectionString();
            if (string.IsNullOrEmpty(cs)) return;

            var account = CloudStorageAccount.Parse(cs);
            var tableClient = account.CreateCloudTableClient();
            table = tableClient.GetTableReference(endpointName.Replace(".", ""));
        }

        public void Store(string endpoint, Type messageType)
        {
            if (!tableCreated)
            {
                table.CreateIfNotExists();
                tableCreated = true;
            }

            var type = messageType == null ? string.Empty : messageType.ToString();
            var operation = TableOperation.InsertOrReplace(new SubscriptionEntity(endpoint, type));
            table.Execute(operation);
        }

        public IEnumerable<string> GetSubscribersEndpoints(Type messageType)
        {
            try
            {
                var query = new TableQuery<SubscriptionEntity>();
                var result = table.ExecuteQuery(query);
                return result.Where(x =>
                    string.IsNullOrEmpty(x.MessageType) || x.MessageType == messageType.ToString())
                    .Select(x => x.Endpoint);
            }
            catch (Exception ex)
            {
                log.Error("Failed to get subscriptions", ex);
                return new List<string>();
            }

        }
    }
}