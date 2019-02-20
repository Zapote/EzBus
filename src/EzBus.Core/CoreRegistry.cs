using EzBus.Core.Middleware;
using EzBus.Core.Resolvers;
using EzBus.Core.Routing;
using EzBus.Core.Utils;
using EzBus.ObjectFactory;
using EzBus.Serializers;
using EzBus.Utils;

namespace EzBus.Core
{
    public class CoreRegistry : ServiceRegistry
    {
        public CoreRegistry()
        {
            RegisterHost();
            RegisterBus();
            RegisterChannels();
            RegisterMessageSerializer();
            RegisterMessageHandlers();
            RegisterHandlerCache();
            RegisterMiddlewares();
            RegisterTaskRunner();
            RegistertBusConfig();
            RegisterStartupTasks();
        }

        private void RegisterHost()
        {
            Register<IHost, Host>().As.Singleton();
        }

        private void RegisterBus()
        {
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
            var type = TypeResolver.GetType<IBodySerializer>();
            Register(typeof(IBodySerializer), type).As.Singleton();
        }

        private void RegisterMessageHandlers()
        {
            var assemblyScanner = new AssemblyScanner();
            var types = assemblyScanner.FindTypes<IMessageHandler>();
            foreach (var type in types)
            {
                if (type.IsInterface()) continue;
                Register(type, type, type.FullName);
            }
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
                if (type.IsInterface()) continue;
                if (type.ImplementsInterface(typeof(IPreMiddleware)))
                {
                    Register(typeof(IPreMiddleware), type, type.FullName);
                    continue;
                }
                if (type.ImplementsInterface(typeof(ISystemMiddleware)))
                {
                    Register(typeof(ISystemMiddleware), type, type.FullName);
                    continue;
                }

                Register(typeof(IMiddleware), type, type.FullName);
            }
        }

        private void RegistertBusConfig()
        {
            Register(typeof(IBusConfig), typeof(BusConfig)).As.Singleton();
        }

        private void RegisterStartupTasks()
        {
            var assemblyScanner = new AssemblyScanner();
            var types = assemblyScanner.FindTypes<IStartupTask>();

            foreach (var type in types)
            {
                if (type.IsInterface()) continue;
                Register(typeof(IStartupTask), type, type.FullName).As.Singleton();
            }
        }
    }
}