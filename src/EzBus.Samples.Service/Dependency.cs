using System;

namespace EzBus.Samples.Service
{
    public class Dependency : IDependency
    {
        public Dependency()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
    }
}