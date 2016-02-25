using EzBus.ObjectFactory;

namespace EzBus.Core
{
    public class HostFactory
    {
        public Host Build(IHostConfig hostConfig, IObjectFactory objectFactory)
        {
            return new Host(hostConfig, objectFactory);
        }
    }
}