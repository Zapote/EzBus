using System.Collections.Generic;

namespace EzBus.ObjectFactory
{
    public interface IServiceRegistry
    {
        IEnumerable<IRegistryInstance> Instances { get; }
    }
}