using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;
using EzBus.Core.Utils;
using EzBus.Logging;
using EzBus.Serializers;

namespace EzBus.Core.Serializers
{
    public class XmlMessageSerializer : IMessageSerializer
    {
        private static readonly ILogger log = LogManager.GetLogger(typeof(XmlMessageSerializer));

        public Stream Serialize(object obj)
        {
            var xmlDocument = CreateXmlDocument();
            var name = CreateTypeName(obj);
            var rootElement = xmlDocument.CreateElement(name);

            WriteObject(obj, xmlDocument, rootElement);
            xmlDocument.AppendChild(rootElement);

            return CreateXmlStream(xmlDocument);
        }

        private static Stream CreateXmlStream(XmlDocument xmlDocument)
        {
            var xmlStream = new MemoryStream();
            xmlDocument.Save(xmlStream);
            xmlStream.Flush();
            xmlStream.Position = 0;
            return xmlStream;
        }

        private static void WriteObject(object obj, XmlDocument xmlDocument, XmlNode currentNode)
        {
            var objType = obj.GetType();

            if (objType.IsCollection())
            {
                var enumerable = obj as IEnumerable;
                if (enumerable == null) return;

                foreach (var item in enumerable)
                {
                    var child = xmlDocument.CreateElement(item.GetType().Name);
                    currentNode.AppendChild(child);

                    WriteObject(item, xmlDocument, child);
                }

                return;
            }

            if (objType.IsClass())
            {
                var properties = objType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (var pi in properties)
                {
                    var child = xmlDocument.CreateElement(pi.Name);
                    var propertyValue = pi.GetValue(obj, null);
                    currentNode.AppendChild(child);
                    if (propertyValue == null) continue;
                    WriteObject(propertyValue, xmlDocument, child);
                }
            }
            else
            {
                var text = string.Format(CultureInfo.InvariantCulture, "{0}", obj);
                currentNode.AppendChild(xmlDocument.CreateTextNode(text));
            }
        }

        private static string CreateTypeName(object obj)
        {
            if (obj.GetType().IsCollection())
                return "ArrayOf";
            var name = obj.GetType().Name;
            return name;
        }

        private static XmlDocument CreateXmlDocument()
        {
            var xmlDocument = new XmlDocument();
            var declaration = xmlDocument.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDocument.AppendChild(declaration);
            return xmlDocument;
        }

        public object Deserialize(Stream messageStream, Type messageType)
        {
            var instance = FormatterServices.GetUninitializedObject(messageType);

            try
            {
                var xDoc = XDocument.Load(messageStream);
                WriteToInstance(instance, messageType, xDoc.Root);
            }
            catch (Exception ex)
            {
                log.Fatal("Failed to deserialize message!", ex);
                throw;
            }

            return instance;
        }

        private static void WriteToInstance(object instance, IReflect type, XContainer xContainer)
        {
            foreach (var pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var propertyType = pi.PropertyType;

                if (propertyType.IsValueType())
                {
                    var child = xContainer.Element(pi.Name);
                    if (child == null) continue;
                    if (pi.GetSetMethod(true) == null) continue;
                    var value = XmlValueConverter.Convert(child.Value, propertyType);
                    pi.SetValue(instance, value, null);
                    continue;
                }

                if (propertyType.IsClass())
                {
                    var propertyInstance = FormatterServices.GetUninitializedObject(propertyType);
                    pi.SetValue(instance, propertyInstance, null);
                    WriteToInstance(propertyInstance, propertyType, xContainer.Element(pi.Name));
                    continue;
                }

                if (propertyType.IsCollection())
                {
                    var current = xContainer.Element(pi.Name);
                    if (current == null) return;
                    var itemType = typeof(object);

                    if (propertyType.IsGenericType)
                    {
                        itemType = propertyType.GetGenericArguments()[0];
                    }

                    var list = itemType.CreateGenericList();

                    foreach (var item in current.Elements())
                    {
                        var itemInstance = FormatterServices.GetUninitializedObject(itemType);
                        WriteToInstance(itemInstance, itemType, item);
                        list.Add(itemInstance);
                    }

                    pi.SetValue(instance, list, null);
                }
            }
        }
    }
}
