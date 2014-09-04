using EzBus.Core.Config;
using NUnit.Framework;

namespace EzBus.Core.Test.Config
{
    [TestFixture]
    public class SubscriptionsSectionConfigTest
    {
        private readonly SubscriptionCollection subscriptions = SubscriptionSection.Section.Subscriptions;

        [Test]
        public void Two_subscriptions_should_exist()
        {
            Assert.That(subscriptions[0].Endpoint, Is.EqualTo("acme.sales"));
            Assert.That(subscriptions[1].Endpoint, Is.EqualTo("acme.production"));
        }
    }
}