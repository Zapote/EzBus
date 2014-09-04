using System.Configuration;

namespace EzBus.Core.Config
{
    [ConfigurationCollection(typeof(DestinationElement), AddItemName = "add", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class SubscriptionCollection : ConfigurationElementCollection
    {
        public SubscriptionElement this[int index]
        {
            get { return (SubscriptionElement)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);

                BaseAdd(index, value);
            }
        }

        public void Add(ConfigurationElement element)
        {
            BaseAdd(element);
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            var destinationElement = ((SubscriptionElement)(element));
            return destinationElement.Endpoint;
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new SubscriptionElement();
        }
    }
}