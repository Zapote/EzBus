using EzBus.Core.Resolvers;
using EzBus.Core.Subscription;
using EzBus.Core.Test.TestHelpers;
using NUnit.Framework;

namespace EzBus.Core.Test.Resolvers
{
    [TestFixture]
    public class SubscriptionManagerResolverTest
    {
        [Test]
        public void Resolved_instance_should_be_SubscriptionManager()
        {
            var instance = SubscriptionManagerResolver.GetSubscriptionManager();
            Assert.That(instance, Is.InstanceOf<DefaultSubscriptionManager>());
        }
    }
}