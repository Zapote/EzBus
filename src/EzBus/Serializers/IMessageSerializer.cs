using System;
using System.IO;

namespace EzBus.Serializers
{
    public interface IMessageSerializer
    {
        void Serialize(object message, Stream stream);
        object Deserialize(Stream stream, Type messageType);
    }
}
