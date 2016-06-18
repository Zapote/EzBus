using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using EzBus.Core.Serializers;
using EzBus.Core.Test.TestHelpers;
using NUnit.Framework;

namespace EzBus.Core.Test.Serializers
{
    [TestFixture]
    public class XmlSerializationTest
    {
        private readonly XmlMessageSerializer serializer = new XmlMessageSerializer();
        private Stream result;

        [Test]
        public void DecimalIsSerializedCorrect()
        {
            const decimal elementValue = 1.0m;

            var xDoc = Serialize(elementValue);

            if (xDoc.Root == null) Assert.Fail("Document should not be null");
            Assert.That(xDoc.Root.Name.LocalName, Is.EqualTo(elementValue.GetType().Name));
            Assert.That(xDoc.Descendants("Decimal").First().Value, Is.EqualTo(elementValue.ToString(CultureInfo.InvariantCulture)));
        }

        [Test]
        public void StringIsSerializedCorrect()
        {
            const string elementValue = "FooBar";

            var xDoc = Serialize(elementValue);

            if (xDoc.Root == null) Assert.Fail("Document should not be null");
            Assert.That(xDoc.Root.Name.LocalName, Is.EqualTo(elementValue.GetType().Name));
            Assert.That(xDoc.Descendants("String").First().Value, Is.EqualTo(elementValue));
        }

        [Test]
        public void DateTimeIsSerializedCorrect()
        {
            var elementValue = new DateTime(2001, 12, 31, 22, 45, 30);

            var xDoc = Serialize(elementValue);

            if (xDoc.Root == null) Assert.Fail("Document should not be null");
            Assert.That(xDoc.Root.Name.LocalName, Is.EqualTo(elementValue.GetType().Name));
            Assert.That(xDoc.Descendants("DateTime").First().Value, Is.EqualTo(elementValue.ToString(CultureInfo.InvariantCulture)));
        }

        [Test]
        public void Collection_is_serialized_correct()
        {
            IEnumerable<string> elementValue = new List<string> { "1", "2", "3" };

            var xDoc = Serialize(elementValue);

            Assert.That(xDoc.Descendants("String").ElementAt(0).Value, Is.EqualTo("1"));
            Assert.That(xDoc.Descendants("String").ElementAt(1).Value, Is.EqualTo("2"));
            Assert.That(xDoc.Descendants("String").ElementAt(2).Value, Is.EqualTo("3"));
        }

        [Test]
        public void Message_with_collection_is_serialized_correct()
        {
            var message = new TestMessageWithCollection("");
            message.AddData(new TestMessageData { IntValue = 1 });
            message.AddData(new TestMessageData { IntValue = 2 });

            result = serializer.Serialize(message);
            var xDoc = XDocument.Load(result);

            if (xDoc.Root == null) Assert.Fail("Document should not be null");
            Assert.That(xDoc.Root.Name.LocalName, Is.EqualTo("TestMessageWithCollection"));
            Assert.That(xDoc.Root.Descendants().First().Name.LocalName, Is.EqualTo("DataCollection"));
        }

        [Test]
        public void ComplexTypeIsSerializedCorrect()
        {
            const string stringValue = "First_section_is_generic_for_assembly text";
            const int intValue = 1;
            var message = new TestMessage(stringValue) { TestMessageData = { IntValue = intValue } };

            var xDoc = Serialize(message);

            Assert.That(xDoc.Descendants("StringValue").ElementAt(0).Value, Is.EqualTo(stringValue));
            Assert.That(xDoc.Descendants("IntValue").ElementAt(0).Value, Is.EqualTo(intValue.ToString(CultureInfo.InvariantCulture)));
        }

        private XDocument Serialize(object obj)
        {
            result = serializer.Serialize(obj);
            var xDoc = XDocument.Load(result);
            return xDoc;
        }
    }
}
