using System.Linq;
using NUnit.Framework;

namespace EzBus.Core.Test
{
    [TestFixture]
    public class SubscriptionMessageHandlerTest
    {
        private SubscriptionMessageHandler handler;
        private readonly string endpoint = new string(new[] { 'A', 'E', 'P' });
        private readonly InMemorySubscriptionStorage subscriptionStorage = new InMemorySubscriptionStorage();

        [SetUp]
        public void TestSetup()
        {
            handler = new SubscriptionMessageHandler(subscriptionStorage);
        }

        [Test]
        public void Subscribers_are_stored_after_subscription_messages_arrives()
        {
            handler.Handle(new SubscriptionMessage { Endpoint = endpoint });

            var actual = subscriptionStorage.GetSubscribersEndpoints(null).Last();
            Assert.That(actual, Is.EqualTo(endpoint));
        }
    }
}