using System;
using System.IO.Pipes;
using EzBus.Utils;

namespace EzBus.NamnedPipes
{
    public class NamnedPipeSendingChannel : ISendingChannel
    {
        private const int connectTimeout = 1000;

        public void Send(EndpointAddress destination, ChannelMessage channelMessage)
        {
            var serverName = string.IsNullOrEmpty(destination.MachineName) ? @"." : destination.MachineName;
            var stream = new NamedPipeClientStream(serverName, destination.Name);

            try
            {
                stream.Connect(connectTimeout);
            }
            catch (TimeoutException ex)
            {
                throw new ConnectionTimeout($"Failed to connect. Timed out after {connectTimeout} ms.", ex);
            }

            var bodyBytes = channelMessage.BodyStream.ToByteArray();
            var headerBytes = channelMessage.Headers;
            stream.WriteByte((byte)bodyBytes.Length);
            stream.Write(bodyBytes, 0, bodyBytes.Length);
        }
    }

    public class NamnedPipesReceivingChannel : IReceivingChannel
    {
        private NamedPipeServerStream serverStream;
        public Action<ChannelMessage> OnMessage { get; set; }

        public NamnedPipesReceivingChannel() { }

        public void Initialize(EndpointAddress inputAddress, EndpointAddress errorAddress)
        {
            serverStream = new NamedPipeServerStream(inputAddress.Name, PipeDirection.InOut);
            var task = serverStream.WaitForConnectionAsync();
            task.ContinueWith(x =>
            {
                var len = serverStream.ReadByte();
                var buffer = new byte[len];
                serverStream.Read(buffer, 0, len);
            });
        }
    }

    public class ConnectionTimeout : Exception
    {
        public ConnectionTimeout(string message, Exception exception)
            : base(message, exception)
        {

        }
    }
}
