using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace EzBus.WindowsAzure.ServiceBus
{
    public class AzureTableSubscriptionstorage : ISubscriptionStorage
    {
        private CloudTable table;

        public void Initialize(string endpointName)
        {
            var cs = ConnectionStringHelper.GetStorageConnectionString();
            if (string.IsNullOrEmpty(cs)) return;

            var account = CloudStorageAccount.Parse(cs);
            var tableClient = account.CreateCloudTableClient();
            table = tableClient.GetTableReference(endpointName.Replace(".", ""));
            table.CreateIfNotExists();
        }

        public void Store(string endpoint, Type messageType)
        {
            var type = messageType == null ? string.Empty : messageType.ToString();
            var operation = TableOperation.InsertOrReplace(new SubscriptionEntity(endpoint, type));
            table.Execute(operation);
        }

        public IEnumerable<string> GetSubscribersEndpoints(Type messageType)
        {
            var query = new TableQuery<SubscriptionEntity>();
            var result = table.ExecuteQuery(query);
            return result.Where(x =>
                string.IsNullOrEmpty(x.MessageType) || x.MessageType == messageType.ToString())
                .Select(x => x.Endpoint);
        }
    }
}