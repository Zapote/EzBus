using EzBus.Core.Test.TestHelpers;
using NUnit.Framework;

namespace EzBus.Core.Test
{
    [Specification]
    public class When_message_is_sent : BusBaseTest
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
            Assert.That(messageChannel.LastSentDestination, Is.EqualTo(EndpointAddress.Parse(expectedDestination)));
        }
    }

    [Specification]
    public abstract class BaseTest
    {
        [Given]

        public void Setup()
        {
            Given();
            When();
        }

        protected abstract void When();
        protected virtual void Given() { }
    }

    public abstract class BusBaseTest : BaseTest
    {
        protected Bus bus;
        protected FakeMessageChannel messageChannel;
        protected readonly FakeMessageRouting messageRouting = new FakeMessageRouting();

        protected override void Given()
        {
            messageChannel = new FakeMessageChannel();
            bus = new Bus(messageChannel, messageRouting);
        }
    }

    public class GivenAttribute : SetUpAttribute { }

    public class ThenAttribute : TestAttribute { }

    public class SpecificationAttribute : TestFixtureAttribute { }
}
