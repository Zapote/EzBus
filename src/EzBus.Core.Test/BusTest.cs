using EzBus.Core.Test.TestHelpers;
using NUnit.Framework;

namespace EzBus.Core.Test
{
    [TestFixture]
    public class BusTest
    {
        private Bus bus;
        private FakeMessageChannel messageChannel;
        private readonly FakeMessageRouting messageRouting = new FakeMessageRouting();
        private string expectedDestination;

        [SetUp]
        public void TestSetup()
        {
            messageChannel = new FakeMessageChannel();
            bus = new Bus(messageChannel, messageRouting);
        }

        [Test]
        public void Message_is_sent_to_correct_destination()
        {
            expectedDestination = "Endor";
            messageChannel.OnMessageReceived += VerifyMessageDestination;
            messageRouting.AddRoute(typeof(MockMessage).Assembly.GetName().Name, typeof(MockMessage).FullName, expectedDestination);

            bus.Send(new MockMessage("Foo"));
        }

        private void VerifyMessageDestination(object sender, MessageReceivedEventArgs e)
        {
            Assert.That(messageChannel.LastSentDestination, Is.EqualTo(EndpointAddress.Parse(expectedDestination)));
        }
    }
}
