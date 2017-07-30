using System;
using EzBus.Msmq.Subscription;

namespace EzBus.Msmq
{
    public class MsmqSubscriptionStorageStartupTask : IStartupTask
    {
        private readonly ISubscriptionStorage subscriptionStorage;

        public MsmqSubscriptionStorageStartupTask(ISubscriptionStorage subscriptionStorage)
        {
            this.subscriptionStorage = subscriptionStorage ?? throw new ArgumentNullException(nameof(subscriptionStorage));
        }

        public string Name => "MsmqSubscriptionStorage";

        public void Run()
        {
            subscriptionStorage.Initialize();
        }
    }
}