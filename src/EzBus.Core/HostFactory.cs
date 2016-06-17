using EzBus.ObjectFactory;

namespace EzBus.Core
{
    public class HostFactory
    {
        public Host Build(ITaskRunner taskRunner)
        {
            return new Host(taskRunner);
        }
    }
}