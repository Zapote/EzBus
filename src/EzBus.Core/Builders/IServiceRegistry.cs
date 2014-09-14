using System.Collections.Generic;

namespace EzBus.Core.Builders
{
    public interface IServiceRegistry
    {
        IEnumerable<RegistryInstance> Instances { get; }
    }
}