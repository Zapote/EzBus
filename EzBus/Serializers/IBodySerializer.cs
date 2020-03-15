using System;
using System.IO;

namespace EzBus.Serializers
{
    public interface IBodySerializer
    {
        void Serialize(object message, Stream stream);
        object Deserialize(Stream stream, Type messageType);
    }
}
