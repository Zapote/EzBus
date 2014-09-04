using System.Configuration;

namespace EzBus.Core.Config
{
    public class SubscriptionSection : ConfigurationSection
    {
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public SubscriptionCollection Subscriptions
        {
            get { return ((SubscriptionCollection)(base[""])); }
        }

        public static SubscriptionSection Section
        {
            get
            {
                return ConfigurationManager.GetSection("subscriptions") as SubscriptionSection ?? new SubscriptionSection();
            }
        }
    }
}