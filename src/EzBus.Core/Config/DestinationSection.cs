using System.Configuration;

namespace EzBus.Core.Config
{
    public class DestinationSection : ConfigurationSection
    {
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public DestinationCollection Destinations
        {
            get { return ((DestinationCollection)(base[""])); }
        }

        public static DestinationSection Section
        {
            get
            {
                var section = ConfigurationManager.GetSection("destinations") as DestinationSection;
                if (section == null) throw new ConfigurationErrorsException("Unable to retrieve 'destination' section");
                return section;
            }

        }
    }
}
