using System;
using System.IO;

namespace EzBus.RabbitMQ
{
    internal static class StreamExtensions
    {
        public static byte[] ToByteArray(this Stream stream)
        {
            var length = stream.Length > int.MaxValue ? int.MaxValue : Convert.ToInt32(stream.Length);
            var buffer = new byte[length];
            stream.Read(buffer, 0, length);
            return buffer;
        }
    }
}