using EzBus.Core.Resolvers;

namespace EzBus.Core.Test.TestHelpers
{
    public class FakeMessageChannelResolver : IMessageChannelResolver
    {
        public ISendingChannel GetSendingChannel()
        {
            return new FakeMessageChannel();
        }

        public IReceivingChannel GetReceivingChannel()
        {
            return new FakeMessageChannel();
        }
    }
}