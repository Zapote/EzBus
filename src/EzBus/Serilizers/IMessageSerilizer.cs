using System;
using System.IO;

namespace EzBus.Serilizers
{
    public interface IMessageSerilizer
    {
        Stream Serialize(object obj);
        object Deserialize(Stream messageStream, Type messageType);
    }
}
