using EzBus.Config;
using EzBus.Core.Config;
using EzBus.Core.Middleware;
using EzBus.Core.Resolvers;
using EzBus.Core.Routing;
using EzBus.Core.Utils;
using EzBus.ObjectFactory;
using EzBus.Serializers;

namespace EzBus.Core
{
    public class CoreRegistry : ServiceRegistry
    {
        public CoreRegistry()
        {
            RegisterBus();
            RegisterChannels();
            RegisterMessageSerializer();
            RegisterSubscriptions();
            RegisterHandlerCache();
            RegisterMiddlewares();
            RegisterTaskRunner();
        }

        private void RegisterBus()
        {
            Register<IMessageRouting, ConfigurableMessageRouting>().As.Singleton();
            Register<IBus, CoreBus>().As.Singleton();
        }

        private void RegisterChannels()
        {
            Register(typeof(ISendingChannel), TypeResolver.GetType<ISendingChannel>()).As.Singleton();
            Register(typeof(IReceivingChannel), TypeResolver.GetType<IReceivingChannel>()).As.Unique();
            Register(typeof(IPublishingChannel), TypeResolver.GetType<IPublishingChannel>()).As.Unique();
        }

        private void RegisterMessageSerializer()
        {
            var type = TypeResolver.GetType<IMessageSerializer>();
            Register(typeof(IMessageSerializer), type).As.Singleton();
        }

        private void RegisterSubscriptions()
        {
            var subscriptions = SubscriptionSection.Section.Subscriptions;
            RegisterInstance(typeof(ISubscriptionCollection), subscriptions);
        }

        private void RegisterHandlerCache()
        {
            var handlerCache = new HandlerCache();
            handlerCache.Prime();
            RegisterInstance(typeof(IHandlerCache), handlerCache);
        }

        private void RegisterTaskRunner()
        {
            Register<ITaskRunner, TaskRunner>().As.Singleton();
        }

        private void RegisterMiddlewares()
        {
            var assemblyScanner = new AssemblyScanner();
            var types = assemblyScanner.FindTypes<IMiddleware>();

            foreach (var type in types)
            {
                if (type.IsInterface) continue;
                if (type.ImplementsInterface(typeof(IPreMiddleware)))
                {
                    Register(typeof(IPreMiddleware), type, type.FullName).As.Singleton();
                    continue;
                }
                if (type.ImplementsInterface(typeof(ISystemMiddleware)))
                {
                    Register(typeof(ISystemMiddleware), type, type.FullName).As.Singleton();
                    continue;
                }

                Register(typeof(IMiddleware), type, type.FullName).As.Singleton();
            }
        }
    }
}