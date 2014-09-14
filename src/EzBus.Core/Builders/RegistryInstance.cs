using System;

namespace EzBus.Core.Builders
{
    public class RegistryInstance
    {
        public RegistryInstance(Type service, Type implementation)
        {
            Service = service;
            Implementation = implementation;
        }

        public Type Implementation { get; private set; }
        public Type Service { get; private set; }
    }
}