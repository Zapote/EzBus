using System.Configuration;

namespace EzBus.Core.Config
{
    public class SubscriptionElement : ConfigurationElement
    {
        [ConfigurationProperty("endpoint", DefaultValue = "", IsRequired = true)]
        public string Endpoint
        {
            get { return (string)this["endpoint"]; }
            set { this["endpoint"] = value; }
        }
    }
}