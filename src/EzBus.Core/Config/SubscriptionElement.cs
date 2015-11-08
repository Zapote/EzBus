using System.Configuration;
using EzBus.Config;

namespace EzBus.Core.Config
{
    public class SubscriptionElement : ConfigurationElement, ISubscription
    {
        [ConfigurationProperty("endpoint", DefaultValue = "", IsRequired = true)]
        public string Endpoint
        {
            get { return (string)this["endpoint"]; }
            set { this["endpoint"] = value; }
        }
    }
}