namespace EzBus.Core
{
    public class HostFactory
    {
        public Host Build()
        {
            var hostConfig = new HostConfig();
            return new Host(hostConfig);
        }
    }
}