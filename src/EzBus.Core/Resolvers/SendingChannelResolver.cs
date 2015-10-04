namespace EzBus.Core.Resolvers
{
    public class SendingChannelResolver : ResolverBase<ISendingChannel>
    {
        private static readonly SendingChannelResolver resolver = new SendingChannelResolver();

        public static ISendingChannel GetChannel()
        {
            return resolver.GetInstance();
        }
    }
}