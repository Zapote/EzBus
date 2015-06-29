using EzBus.Core.Builders;

namespace EzBus.Core.Resolvers
{
    public class ObjectFactoryResolver
    {
        private static IObjectFactory instance;

        public static IObjectFactory GetObjectFactory()
        {
            return instance ?? (instance = new LightInjectObjectFactory());
        }
    }
}