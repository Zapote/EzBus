using System;

namespace EzBus.Samples.Service
{
    public class OtherDependency : IOtherDependency
    {
        public OtherDependency()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }
    }
}