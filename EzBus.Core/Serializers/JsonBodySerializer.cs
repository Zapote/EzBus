using System;
using System.IO;
using System.Text.Json;
using EzBus.Serializers;

namespace EzBus.Core.Serializers
{
    public class JsonBodySerializer : IBodySerializer
    {
        public void Serialize(object message, Stream stream)
        {
            JsonSerializer.Serialize(stream, message);
            stream.Position = 0;
        }

        public object Deserialize(Stream stream, Type messageType)
        {
            ArgumentNullException.ThrowIfNull(stream);
            ArgumentNullException.ThrowIfNull(messageType);

            try
            {
                return JsonSerializer.Deserialize(stream, messageType);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to deserialize message of type {messageType.Name}", ex);
            }
        }
    }
}

