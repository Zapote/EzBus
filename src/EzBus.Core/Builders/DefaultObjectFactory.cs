using System;

namespace EzBus.Core.Builders
{
    public class DefaultObjectFactory : IObjectFactory
    {
        private readonly IServiceContainer container = new ServiceContainer();

        public object CreateInstance(Type type)
        {
            return container.TryGetInstance(type);
        }

        public void Register<TService, TImplementation>()
            where TImplementation : TService
        {
            container.Register<TService, TImplementation>();
        }
    }
}