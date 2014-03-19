using System.Configuration;

namespace EzBus.Core.Config
{
    public class DestinationElement : ConfigurationElement
    {
        [ConfigurationProperty("Assembly", DefaultValue = "", IsRequired = true)]
        public string Assembly
        {
            get { return (string)this["Assembly"]; }
            set { this["Assembly"] = value; }
        }

        [ConfigurationProperty("Endpoint", DefaultValue = "", IsRequired = true)]
        public string Endpoint
        {
            get { return (string)this["Endpoint"]; }
            set { this["Endpoint"] = value; }
        }

        [ConfigurationProperty("Message", DefaultValue = "", IsRequired = false)]
        public string Message
        {
            get { return (string)this["Message"]; }
            set { this["Message"] = value; }
        }
    }
}