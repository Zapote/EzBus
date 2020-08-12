using System;
using System.IO;
using System.Linq;
using EzBus.Core.Serializers;
using EzBus.Core.Test.TestHelpers;
using Xunit;

namespace EzBus.Core.Test.Serializers
{
    public class JsonDeserializationTest
    {
        private readonly TestMessage message;
        private readonly DateTime dateTimeValue = new DateTime(2011, 12, 23, 13, 37, 0);
        private const string stringValue = "FooBar";
        private const int intValue = 1337;
        private const int nullableIntValue = 1338;

        public JsonDeserializationTest()
        {
            var serializer = new JsonBodySerializer();
            var json = $@"{{ 
                                StringValue : '{stringValue}', 
                                TestEnum : '{TestEnum.Bar}',
                                NullableIntValue : {nullableIntValue},
                                DateTimeValue : '2011-12-23T13:37:00',
                                TestMessageData : 
                                {{
                                    IntValue : {intValue}
                                }}
                          }}";

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
            Assert.Equal(nullableIntValue, message.NullableIntValue);
        }

        [Fact]
        public void DateTimeValue_should_be_set()
        {
            Assert.Equal(dateTimeValue, message.DateTimeValue);
        }
    }
}