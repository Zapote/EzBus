using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using EzBus.Serializers;
using Newtonsoft.Json;

namespace EzBus.Core.Serializers
{
    public class JsonBodySerializer : IBodySerializer
    {
        private readonly JsonSerializer serializer = new JsonSerializer();

        public void Serialize(object message, Stream stream)
        {
            var streamWriter = new StreamWriter(stream);
            var jsonTextWriter = new JsonTextWriter(streamWriter)
            {
                Formatting = Formatting.None
            };
            serializer.Serialize(jsonTextWriter, message);
            jsonTextWriter.Flush();
            stream.Position = 0;
        }

        public object Deserialize(Stream stream, Type messageType)
        {
            var textReader = new StreamReader(stream);
            var jsonReader = new JsonTextReader(textReader);

            return serializer.Deserialize(jsonReader, messageType);
        }
    }
}

