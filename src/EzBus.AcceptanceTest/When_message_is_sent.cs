using System.Reflection;
using EzBus.AcceptanceTest.Specifications;
using EzBus.AcceptanceTest.TestHelpers;
using EzBus.Core;
using NUnit.Framework;

namespace EzBus.AcceptanceTest
{
    [Specification]
    public class When_message_is_sent : BusSpecificationBase
    {
        private const string expectedDestination = "Endor";

        protected override void Given()
        {
            base.Given();
            messageRouting.AddRoute(typeof(TestMessage).GetTypeInfo().Assembly.GetName().Name, typeof(TestMessage).FullName, expectedDestination);
        }

        protected override void When()
        {
            bus.Send(new TestMessage());
        }

        [Then]
        public void Then_the_message_should_end_up_in_correct_destination()
        {
            Assert.That(FakeMessageChannel.LastSentDestination, Is.EqualTo(EndpointAddress.Parse(expectedDestination)));
        }
    }
}
