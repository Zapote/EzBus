using System;
using System.IO;
using System.Text;
using EzBus.Serializers;
using Newtonsoft.Json;

namespace EzBus.Core.Serializers
{
    public class JsonMessageSerializer : IMessageSerializer
    {
        private readonly JsonSerializer serializer = new JsonSerializer();

        public void Serialize(object message, Stream stream)
        {
            var streamWriter = new StreamWriter(stream, Encoding.UTF8);
            var jsonTextWriter = new JsonTextWriter(streamWriter)
            {
                Formatting = Formatting.None
            };
            serializer.Serialize(jsonTextWriter, message);
            jsonTextWriter.Flush();
        }

        public object Deserialize(Stream stream, Type messageType)
        {
            var textReader = new StreamReader(stream);
            var jsonReader = new JsonTextReader(textReader);

            return serializer.Deserialize(jsonReader, messageType);
        }
    }
}
