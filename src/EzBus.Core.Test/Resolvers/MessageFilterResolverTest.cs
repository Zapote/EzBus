using System.Linq;
using EzBus.Core.Builders;
using EzBus.Core.Resolvers;
using NUnit.Framework;

namespace EzBus.Core.Test.Resolvers
{
    [TestFixture]
    public class MessageFilterResolverTest
    {
        [Test]
        public void Receiving_channel_should_be_FakeMessageChannel()
        {
            var messageFilters = MessageFilterResolver.GetMessageFilters(new LightInjectObjectFactory());
            Assert.That(messageFilters.Count(), Is.EqualTo(1));
        }
    }
}