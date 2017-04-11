using System.Linq;
using EzBus.Core.Serializers;
using EzBus.Core.Test.TestHelpers;
using NUnit.Framework;

namespace EzBus.Core.Test.Serializers
{
    [TestFixture]
    public class XmlDeserializationTest
    {
        private XmlMessageSerializer serializer;
        private TestMessageWithCollection message;
        private const string stringValue = "FooBar";
        private const int mockDataIntValue = int.MaxValue;

        [OneTimeSetUp]
        public void FixtureSetup()
        {
            serializer = new XmlMessageSerializer();
            var serializationMessage = new TestMessageWithCollection(stringValue)
            {
                TestMessageData = { IntValue = mockDataIntValue }
            };

            serializationMessage.AddData(new TestMessageData { IntValue = 1 });
            serializationMessage.AddData(new TestMessageData { IntValue = 2 });

            var xml = serializer.Serialize(serializationMessage);
            message = serializer.Deserialize(xml, typeof(TestMessageWithCollection)) as TestMessageWithCollection;
        }

        [Test]
        public void Message_should_be_created()
        {
            Assert.That(message, Is.Not.Null);
        }

        [Test]
        public void StringValue_should_be_set()
        {
            Assert.That(message.StringValue, Is.EqualTo(stringValue));
        }

        [Test]
        public void IntValue_should_be_set()
        {
            Assert.That(message.TestMessageData.IntValue, Is.EqualTo(mockDataIntValue));
        }

        [Test]
        public void NullableIntValue_should_be_set()
        {
            Assert.That(message.NullableIntValue, Is.Null);
        }

        [Test]
        public void Collection_should_be_set()
        {
            Assert.That(message.DataCollection.Count(), Is.EqualTo(2));
        }
    }
}