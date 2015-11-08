using System.Configuration;

namespace EzBus.Core.Config
{
    public class SubscriptionSection : ConfigurationSection
    {
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public SubscriptionCollection Subscriptions => ((SubscriptionCollection)(base[""]));
        public static SubscriptionSection Section => ConfigurationManager.GetSection("subscriptions") as SubscriptionSection ?? new SubscriptionSection();
    }
}