using EzBus.Core.Builders;
using EzBus.Core.Resolvers;
using EzBus.Serializers;

namespace EzBus.Core
{
    public class CoreRegistry : ServiceRegistry
    {
        public CoreRegistry()
        {
            RegisterSendingChannel();
            RegisterReceivingChannel();
            RegisterMessageSerializer();
        }

        private void RegisterSendingChannel()
        {
            var type = TypeResolver.Get<ISendingChannel>();
            Register(typeof(ISendingChannel), type).As.Singleton();
        }

        private void RegisterReceivingChannel()
        {
            var type = TypeResolver.Get<IReceivingChannel>();
            Register(typeof(IReceivingChannel), type).As.Unique();
        }

        private void RegisterMessageSerializer()
        {
            var type = TypeResolver.Get<IMessageSerializer>();
            Register(typeof(IMessageSerializer), type).As.Singleton();
        }
    }
}