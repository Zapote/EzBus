using System;

namespace EzBus.Msmq.Subscription
{
    public class SubscriptionManagerStartup : IStartupTask
    {
        private readonly IMsmqSubscriptionStorage msmqSubscriptionStorage;

        public SubscriptionManagerStartup(IMsmqSubscriptionStorage msmqSubscriptionStorage)
        {
            if (msmqSubscriptionStorage == null) throw new ArgumentNullException(nameof(msmqSubscriptionStorage));
            this.msmqSubscriptionStorage = msmqSubscriptionStorage;
        }

        public void Run()
        {
            msmqSubscriptionStorage.Initialize();
        }
    }
}