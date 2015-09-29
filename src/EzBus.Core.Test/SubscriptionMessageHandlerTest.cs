using System.Linq;
using EzBus.Core.Subscription;
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
            handler = new SubscriptionMessageHandler();
        }

        [Test]
        public void Subscriber_shall_be_stored_in_storage()
        {
            handler.Handle(new SubscriptionMessage { Endpoint = endpoint });

            var actual = subscriptionStorage.GetSubscribersEndpoints(null).Last();

            Assert.That(actual, Is.EqualTo(endpoint));
        }
    }
}