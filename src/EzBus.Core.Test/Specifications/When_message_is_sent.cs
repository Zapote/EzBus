using EzBus.Core.Test.TestHelpers;
using NUnit.Framework;

namespace EzBus.Core.Test.Specifications
{
    [Specification]
    public class When_message_is_sent : BusSpecificationBase
    {
        private const string expectedDestination = "Endor";

        protected override void When()
        {
            bus.Send(new MockMessage("Foo"));
        }

        protected override void Given()
        {
            base.Given();
            messageRouting.AddRoute(typeof(MockMessage).Assembly.GetName().Name, typeof(MockMessage).FullName, expectedDestination);
        }

        [Then]
        public void Then_the_message_should_end_up_in_correct_destination()
        {
            Assert.That(FakeMessageChannel.LastSentDestination, Is.EqualTo(EndpointAddress.Parse(expectedDestination)));
        }
    }
}
