namespace EzBus.Core.Resolvers
{
    public class PublishingChannelResolver : ResolverBase<IPublishingChannel>
    {
        private static readonly PublishingChannelResolver resolver = new PublishingChannelResolver();

        public static IPublishingChannel GetChannel()
        {
            return resolver.GetInstance();
        }
    }
}