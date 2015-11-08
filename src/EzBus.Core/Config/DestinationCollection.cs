using System.Configuration;
using EzBus.Config;

namespace EzBus.Core.Config
{
    [ConfigurationCollection(typeof(DestinationElement), AddItemName = "add", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class DestinationCollection : ConfigurationElementCollection, IDestinationCollection
    {
        public IDestination this[int index]
        {
            get { return (DestinationElement)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);

                BaseAdd(index, (DestinationElement)value);
            }
        }

        public void Add(ConfigurationElement element)
        {
            BaseAdd(element);
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            var destinationElement = ((DestinationElement)(element));
            return destinationElement.Assembly + destinationElement.Message;
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new DestinationElement();
        }
    }
}