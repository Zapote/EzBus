using EzBus.Core.Resolvers;

namespace EzBus.Core
{
    public class HostFactory
    {
        public Host Build(IHostConfig hostConfig)
        {
            var objectFactory = ObjectFactoryResolver.Get();
            return new Host(hostConfig, objectFactory);
        }
    }
}