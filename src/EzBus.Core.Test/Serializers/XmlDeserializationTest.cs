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
        private MockMessageWithCollection message;
        private const string stringValue = "FooBar";
        private const int mockDataIntValue = int.MaxValue;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            serializer = new XmlMessageSerializer();
            var serializationMessage = new MockMessageWithCollection(stringValue)
            {
                MockData = { IntValue = mockDataIntValue }
            };

            serializationMessage.AddData(new MockData { IntValue = 1 });
            serializationMessage.AddData(new MockData { IntValue = 2 });

            var xml = serializer.Serialize(serializationMessage);
            message = serializer.Deserialize(xml, typeof(MockMessageWithCollection)) as MockMessageWithCollection;
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
            Assert.That(message.MockData.IntValue, Is.EqualTo(mockDataIntValue));
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