using System.Linq;
using EzBus.Core.Serializers;
using EzBus.Core.Test.TestHelpers;
using Xunit;

namespace EzBus.Core.Test.Serializers
{
    public class XmlAnonymousDeserializationTest
    {
        private readonly dynamic message;
        private const string stringValue = "FooBar";
        private const int intValue = int.MaxValue;

        public XmlAnonymousDeserializationTest()
        {
            var serializer = new XmlMessageSerializer();
            var serializationMessage = new
            {
                stringValue,
                data = new { intValue }
            };

            var xml = serializer.Serialize(serializationMessage);
            message = serializer.Deserialize(xml, null);
        }

        [Fact(Skip = "pending")]
        public void Message_should_be_created()
        {
            Assert.NotNull(message);
        }

        [Fact(Skip = "pending")]
        public void StringValue_should_be_set()
        {
            Assert.Equal(stringValue, message.StringValue);
        }

        [Fact(Skip = "pending")]
        public void IntValue_should_be_set()
        {
            Assert.Equal(intValue, message.TestMessageData.IntValue);
        }

        [Fact(Skip = "pending")]
        public void NullableIntValue_should_be_set()
        {
            Assert.Null(message.NullableIntValue);
        }

        [Fact(Skip = "pending")]
        public void Collection_should_be_set()
        {
            Assert.Equal(2, message.DataCollection.Count());
        }
    }

    public class XmlDeserializationTest
    {
        private readonly TestMessageWithCollection message;
        private const string stringValue = "FooBar";
        private const int intValue = int.MaxValue;

        public XmlDeserializationTest()
        {
            var serializer = new XmlMessageSerializer();
            var serializationMessage = new TestMessageWithCollection(stringValue)
            {
                TestMessageData = { IntValue = intValue }
            };

            serializationMessage.AddData(new TestMessageData { IntValue = 1 });
            serializationMessage.AddData(new TestMessageData { IntValue = 2 });

            var xml = serializer.Serialize(serializationMessage);
            message = serializer.Deserialize(xml, typeof(TestMessageWithCollection)) as TestMessageWithCollection;
        }

        [Fact]
        public void Message_should_be_created()
        {
            Assert.NotNull(message);
        }

        [Fact]
        public void StringValue_should_be_set()
        {
            Assert.Equal(stringValue, message.StringValue);
        }

        [Fact]
        public void IntValue_should_be_set()
        {
            Assert.Equal(intValue, message.TestMessageData.IntValue);
        }

        [Fact]
        public void NullableIntValue_should_be_set()
        {
            Assert.Null(message.NullableIntValue);
        }

        [Fact]
        public void Collection_should_be_set()
        {
            Assert.Equal(2, message.DataCollection.Count());
        }
    }
}