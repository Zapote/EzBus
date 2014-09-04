using System.Configuration;

namespace EzBus.Core.Config
{
    public class DestinationElement : ConfigurationElement
    {
        [ConfigurationProperty("assembly", DefaultValue = "", IsRequired = true)]
        public string Assembly
        {
            get { return (string)this["assembly"]; }
            set { this["assembly"] = value; }
        }

        [ConfigurationProperty("endpoint", DefaultValue = "", IsRequired = true)]
        public string Endpoint
        {
            get { return (string)this["endpoint"]; }
            set { this["endpoint"] = value; }
        }

        [ConfigurationProperty("message", DefaultValue = "", IsRequired = false)]
        public string Message
        {
            get { return (string)this["message"]; }
            set { this["message"] = value; }
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Message) ? Assembly : string.Format("{0}, {1}", Assembly, Message);
        }
    }
}