﻿    using EzBus.Msmq.Subscription;
using EzBus.ObjectFactory;

namespace EzBus.Msmq
{
    public class MsmqRegistry : ServiceRegistry
    {
        public MsmqRegistry()
        {
            Register<ISubscriptionStorage, MsmqSubscriptionStorage>().As.Singleton();
        }
    }
}
