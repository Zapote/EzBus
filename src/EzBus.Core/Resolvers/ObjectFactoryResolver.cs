using EzBus.Core.Utils;
using EzBus.ObjectFactory;

namespace EzBus.Core.Resolvers
{
    public class ObjectFactoryResolver
    {
        private static IObjectFactory instance;
        private static readonly object syncRoot = new object();

        public static IObjectFactory Get()
        {
            lock (syncRoot)
            {
                if (instance != null) return instance;
                var type = TypeResolver.GetType<IObjectFactory>();
                instance = (IObjectFactory)type.CreateInstance();
                instance.Initialize();
            }

            return instance;
        }
    }
}