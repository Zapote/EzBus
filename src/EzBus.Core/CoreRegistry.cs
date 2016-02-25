using EzBus.Config;
using EzBus.Core.Config;
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
            RegisterHostConfig();
            RegisterHandlerCache();
            RegisterMessageFilters();
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

        private void RegisterHostConfig()
        {
            RegisterInstance(typeof(IHostConfig), new HostConfig());
        }

        private void RegisterHandlerCache()
        {
            Register(typeof(IHandlerCache), typeof(HandlerCache)).As.Singleton();
        }

        private void RegisterTaskRunner()
        {
            Register<ITaskRunner, TaskRunner>().As.Singleton();
        }

        private void RegisterMessageFilters()
        {
            var assemblyScanner = new AssemblyScanner();
            var types = assemblyScanner.FindTypes<IMessageFilter>();
            foreach (var type in types)
            {
                Register(typeof(IMessageFilter), type);
            }
        }
    }
}