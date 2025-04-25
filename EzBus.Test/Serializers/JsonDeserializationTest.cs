using EzBus.Core.Serializers;
using EzBus.Core.Test.TestHelpers;

namespace EzBus.Core.Test.Serializers
{
    public class JsonDeserializationTest
    {
        private readonly TestMessage message = new();

        public JsonDeserializationTest()
        {
            var serializer = new JsonBodySerializer();
            var json = """
                {"StringValue": "FooBar",
                    "TestMessageData": {
                        "IntValue": 1337,
                        "NullableIntValue": 1338
                    },
                    "TestEnum": 1,
                    "NullableIntValue": 1338,
                    "DateTimeValue": "2011-12-23T13:37:00"
                }
                """;

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(json);
            writer.Flush();
            stream.Position = 0;

            message = serializer.Deserialize(stream, typeof(TestMessage)) as TestMessage;
        }

        [Fact]
        public void Message_should_be_created()
        {
            Assert.NotNull(message);
        }

        [Fact]
        public void StringValue_should_be_set()
        {
            Assert.Equal("FooBar", message.StringValue);
        }

        [Fact]
        public void IntValue_should_be_set()
        {
            Assert.Equal(1337, message.TestMessageData.IntValue);
        }

        [Fact]
        public void NullableIntValue_should_be_set()
        {
            Assert.Equal(1338, message.NullableIntValue);
        }

        [Fact]
        public void DateTimeValue_should_be_set()
        {
            Assert.Equal(new DateTime(2011, 12, 23, 13, 37, 0), message.DateTimeValue);
        }
    }
}