using System;

namespace EzBus.Core.Serializers
{
    public class SerializationException : Exception
    {
        public SerializationException(string message) : base(message)
        {

        }
    }
}