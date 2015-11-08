using EzBus.Core.Resolvers;

namespace EzBus.Core
{
    public class HostFactory
    {
        public Host Build()
        {
            var hostConfig = new HostConfig();
            var objectFactory = ObjectFactoryResolver.Get();
            return new Host(hostConfig, objectFactory);
        }
    }
}