namespace EzBus.Core.Resolvers
{
    public class ReceivingChannelResolver : ResolverBase<IReceivingChannel>
    {
        private static readonly ReceivingChannelResolver resolver = new ReceivingChannelResolver();

        public static IReceivingChannel GetChannel()
        {
            return resolver.GetInstance();
        }
    }
}