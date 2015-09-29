using EzBus.Core.Subscription;
using EzBus.Core.Test.TestHelpers;

namespace EzBus.Core.Test.Specifications
{
    public abstract class BusSpecificationBase : SpecificationBase
    {
        protected CoreBus bus;
        protected InMemoryMessageChannel messageChannel;
        protected readonly FakeMessageRouting messageRouting = new FakeMessageRouting();
        protected InMemorySubscriptionStorage subscriptionStorage;

        protected override void Given()
        {
            messageChannel = new InMemoryMessageChannel();
            subscriptionStorage = new InMemorySubscriptionStorage();
            bus = new CoreBus(messageChannel, messageRouting, subscriptionStorage);
        }
    }
}