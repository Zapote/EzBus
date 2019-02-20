using System;
using System.IO;
using System.Text;
using EzBus.Core.Serializers;
using EzBus.Core.Test.TestHelpers;
using Newtonsoft.Json.Linq;
using Xunit;

namespace EzBus.Core.Test.Serializers
{
    public class JsonSerializationTest
    {
        private readonly JsonBodySerializer serializer = new JsonBodySerializer();

        [Fact]
        public void DecimalIsSerializedCorrect()
        {
            const decimal value = 1.0m;

            var json = Serialize(value);

            Assert.Equal("1.0", json);
        }

        [Fact]
        public void StringIsSerializedCorrect()
        {
            const string value = "FooBar";

            var json = Serialize(value);

            Assert.Equal("\"FooBar\"", json);
        }

        [Fact]
        public void DateTimeIsSerializedCorrect()
        {
            var value = new DateTime(2001, 12, 31, 22, 45, 30);

            var json = Serialize(value);

            Assert.Equal("\"2001-12-31T22:45:30\"", json);
        }

        [Fact]
        public void Collection_is_serialized_correct()
        {
            var values = new[] { 1, 2, 3 };

            var json = Serialize(values);
            var array = JArray.Parse(json);

            Assert.Equal(1, array[0]);
            Assert.Equal(2, array[1]);
            Assert.Equal(3, array[2]);
        }

        [Fact]
        public void Message_with_collection_is_serialized_correct()
        {
            var message = new
            {
                Data = new[] { 1, 2, 3 }
            };

            var json = Serialize(message);
            var obj = JObject.Parse(json);

            Assert.Equal(1, obj["Data"][0]);
            Assert.Equal(2, obj["Data"][1]);
            Assert.Equal(3, obj["Data"][2]);
        }

        [Fact]
        public void ComplexTypeIsSerializedCorrect()
        {
            const string stringValue = "First_section_is_generic_for_assembly text";
            const int intValue = 1;
            var message = new
            {
                StringValue = stringValue,
                IntValue = intValue,
                EnumValue = TestEnum.Bar
            };

            var json = Serialize(message);
            var obj = JObject.Parse(json);

            Assert.Equal(stringValue, obj["StringValue"]);
            Assert.Equal(intValue, obj["IntValue"]);
            Assert.Equal("1", obj["EnumValue"]);
        }

        private string Serialize(object obj)
        {
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(obj, stream);
                stream.Position = 0;
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
