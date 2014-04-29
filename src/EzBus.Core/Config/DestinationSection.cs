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
                return ConfigurationManager.GetSection("destinations") as DestinationSection ?? new DestinationSection();
            }
        }
    }
}
