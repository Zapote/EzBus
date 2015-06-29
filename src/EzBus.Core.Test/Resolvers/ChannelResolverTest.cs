using EzBus.Core.Resolvers;
using EzBus.Core.Test.TestHelpers;
using NUnit.Framework;

namespace EzBus.Core.Test.Resolvers
{
    [TestFixture]
    public class ChannelResolverTest
    {
        [Test]
        public void Receiving_channel_should_be_FakeMessageChannel()
        {
            var channel = ChannelResolver.GetReceivingChannel();
            Assert.That(channel, Is.InstanceOf<FakeMessageChannel>());
        }

        [Test]
        public void Sending_channel_should_be_FakeMessageChannel()
        {
            var channel = ChannelResolver.GetSendingChannel();
            Assert.That(channel, Is.InstanceOf<FakeMessageChannel>());
        }
    }
}
