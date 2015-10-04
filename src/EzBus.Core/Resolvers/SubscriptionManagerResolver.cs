using EzBus.Core.Subscription;

namespace EzBus.Core.Resolvers
{
    public class SubscriptionManagerResolver : ResolverBase<ISubscriptionManager>
    {
        private static readonly SubscriptionManagerResolver instance = new SubscriptionManagerResolver();

        public static ISubscriptionManager GetSubscriptionManager()
        {
            return instance.GetInstance();
        }
    }
}