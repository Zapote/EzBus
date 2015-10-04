using EzBus.Serializers;

namespace EzBus.Core.Resolvers
{
    public class MessageSerlializerResolver : ResolverBase<IMessageSerializer>
    {
        private static readonly MessageSerlializerResolver resolver = new MessageSerlializerResolver();

        public static IMessageSerializer GetSerializer()
        {
            return resolver.GetInstance();
        }
    }
}