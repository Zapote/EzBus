using System;
using System.IO;

namespace EzBus.Serializers
{
    public interface IMessageSerializer
    {
        Stream Serialize(object obj, string name = null);
        object Deserialize(Stream messageStream, Type messageType);
    }
}
