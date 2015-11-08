using EzBus.Core.Utils;

namespace EzBus.Core.Resolvers
{
    public class ObjectFactoryResolver
    {
        private static IObjectFactory instance;

        public static IObjectFactory Get()
        {
            if (instance != null) return instance;

            var type = TypeResolver.Get<IObjectFactory>();
            instance = (IObjectFactory)type.CreateInstance();
            instance.Initialize();

            return instance;
        }
    }
}