using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using EzBus.Core.Serializers;
using EzBus.Core.Test.TestHelpers;
using Xunit;

namespace EzBus.Core.Test.Serializers
{
    public class XmlSerializationTest
    {
        private readonly XmlMessageSerializer serializer = new XmlMessageSerializer();
        private Stream result;

        [Fact]
        public void DecimalIsSerializedCorrect()
        {
            const decimal elementValue = 1.0m;

            var xDoc = Serialize(elementValue);

            if (xDoc.Root == null) throw new Exception("Document should not be null");
            Assert.Equal(elementValue.GetType().Name, xDoc.Root.Name.LocalName);
            Assert.Equal(elementValue.ToString(CultureInfo.InvariantCulture), xDoc.Descendants("Decimal").First().Value);
        }

        [Fact]
        public void StringIsSerializedCorrect()
        {
            const string elementValue = "FooBar";

            var xDoc = Serialize(elementValue);

            if (xDoc.Root == null) throw new Exception("Document should not be null");
            Assert.Equal(elementValue.GetType().Name, xDoc.Root.Name.LocalName);
            Assert.Equal(elementValue, xDoc.Descendants("String").First().Value);
        }

        [Fact]
        public void DateTimeIsSerializedCorrect()
        {
            var elementValue = new DateTime(2001, 12, 31, 22, 45, 30);

            var xDoc = Serialize(elementValue);

            if (xDoc.Root == null) throw new Exception("Document should not be null");
            Assert.Equal(elementValue.GetType().Name, xDoc.Root.Name.LocalName);
            Assert.Equal(elementValue.ToString(CultureInfo.InvariantCulture), xDoc.Descendants("DateTime").First().Value);
        }

        [Fact]
        public void Collection_is_serialized_correct()
        {
            IEnumerable<string> elementValue = new List<string> { "1", "2", "3" };

            var xDoc = Serialize(elementValue);

            Assert.Equal("1", xDoc.Descendants("String").ElementAt(0).Value);
            Assert.Equal("2", xDoc.Descendants("String").ElementAt(1).Value);
            Assert.Equal("3", xDoc.Descendants("String").ElementAt(2).Value);
        }

        [Fact]
        public void Message_with_collection_is_serialized_correct()
        {
            var message = new TestMessageWithCollection("");
            message.AddData(new TestMessageData { IntValue = 1 });
            message.AddData(new TestMessageData { IntValue = 2 });

            result = serializer.Serialize(message);
            var xDoc = XDocument.Load(result);

            if (xDoc.Root == null) throw new Exception("Document should not be null");
            Assert.Equal("TestMessageWithCollection", xDoc.Root.Name.LocalName);
            Assert.Equal("DataCollection", xDoc.Root.Descendants().First().Name.LocalName);
        }

        [Fact]
        public void ComplexTypeIsSerializedCorrect()
        {
            const string stringValue = "First_section_is_generic_for_assembly text";
            const int intValue = 1;
            var message = new TestMessage(stringValue) { TestMessageData = { IntValue = intValue } };

            var xDoc = Serialize(message);

            Assert.Equal(stringValue, xDoc.Descendants("StringValue").ElementAt(0).Value);
            Assert.Equal(intValue.ToString(CultureInfo.InvariantCulture), xDoc.Descendants("IntValue").ElementAt(0).Value);
        }

        [Fact]
        public void Anonymous_class_is_serialized_correct()
        {
            const int number = 123;
            var message = new { number, child = new { number } };

            var xDoc = Serialize(message, "DynamicMessage");

            Assert.Equal(number.ToString(), xDoc.Descendants("number").ElementAt(0).Value);
            Assert.Equal(number.ToString(), xDoc.Descendants("child").Descendants("number").ElementAt(0).Value);
        }

        private XDocument Serialize(object obj, string name = null)
        {
            result = serializer.Serialize(obj, name);
            var xDoc = XDocument.Load(result);
            return xDoc;
        }
    }
}
