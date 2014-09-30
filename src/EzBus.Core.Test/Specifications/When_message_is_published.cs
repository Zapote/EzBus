using EzBus.Core.Test.TestHelpers;
using NUnit.Framework;

namespace EzBus.Core.Test.Specifications
{
    [Specification]
    public class When_message_is_published : BusSpecificationBase
    {
        private const string subscriberOne = "Endor";
        private const string subscriberTwo = "Felucia";
        private const string subscriberThree = "void";

        protected override void Given()
        {
            base.Given();

            FakeMessageChannel.Reset();

            subscriptionStorage.Store(subscriberOne, null);
            subscriptionStorage.Store(subscriberTwo, typeof(MockMessage));
            subscriptionStorage.Store(subscriberThree, typeof(object));
        }

        protected override void When()
        {
            bus.Publish(new MockMessage("Foo"));
        }

        [Then]
        public void Then_the_message_should_be_sent_to_two_endpoints()
        {
            var dests = FakeMessageChannel.GetSentDestinations();
            Assert.That(dests, Contains.Item(new EndpointAddress(subscriberOne)));
            Assert.That(dests, Contains.Item(new EndpointAddress(subscriberTwo)));
        }
    }
}