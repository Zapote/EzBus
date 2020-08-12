using System.Reflection;
using EzBus.AcceptanceTest.Specifications;
using EzBus.AcceptanceTest.TestHelpers;
using Xunit;

namespace EzBus.AcceptanceTest
{
    public class When_message_is_sent 
    {
        private const string expectedDestination = "endor";

        public When_message_is_sent()
        {
            //bus.Send(expectedDestination, new TestMessage());
        }

        [Then]
        public void Then_the_message_should_end_up_in_correct_destination()
        {
           //Assert.True(FakeMessageChannel.HasBeenSentToDestination(expectedDestination));
        }
    }
}
