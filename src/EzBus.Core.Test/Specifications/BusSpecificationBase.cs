using EzBus.Core.Test.TestHelpers;

namespace EzBus.Core.Test.Specifications
{
    public abstract class BusSpecificationBase : SpecificationBase
    {
        protected Bus bus;
        protected FakeMessageChannel messageChannel;
        protected readonly FakeMessageRouting messageRouting = new FakeMessageRouting();
        protected InMemorySubscriptionStorage subscriptionStorage;

        protected override void Given()
        {
            messageChannel = new FakeMessageChannel();
            subscriptionStorage = new InMemorySubscriptionStorage();
            bus = new Bus(messageChannel, messageRouting, subscriptionStorage);
        }
    }
}