using System;
using System.IO;
using EzBus.Serializers;
using Newtonsoft.Json;

namespace EzBus.Newtonsoft.Json.NET
{
    public class JsonMessageSerializer : IMessageSerializer
    {
        public Stream Serialize(object obj)
        {
            var json = JsonConvert.SerializeObject(obj, Formatting.None);
            return GenerateStreamFromString(json);
        }

        public object Deserialize(Stream messageStream, Type messageType)
        {
            var reader = new StreamReader(messageStream);
            var json = reader.ReadToEnd();
            return JsonConvert.DeserializeObject(json, messageType);
        }

        public Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
