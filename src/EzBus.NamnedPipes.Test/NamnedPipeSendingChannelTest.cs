using System;
using System.IO;
using EzBus.Core.Serializers;
using Xunit;

namespace EzBus.NamnedPipes.Test
{
    public class NamnedPipeSendingChannelTest
    {
        private readonly NamnedPipeSendingChannel channel = new NamnedPipeSendingChannel();
        private readonly JsonBodySerializer serializer = new JsonBodySerializer();
        private readonly EndpointAddress destination = new EndpointAddress("globex.server");

        [Fact]
        public void Throws_exception_when_failed_to_connect()
        {
            var channelMessage = CreateChannelMessage();

            try
            {
                channel.Send(destination, channelMessage);
            }
            catch (ConnectionTimeout)
            {
                return;
            }

            throw new Exception("Should throw connection timeout");
        }

        [Fact]
        public void Sends_when_connected()
        {
            var rec = new NamnedPipesReceivingChannel();
            rec.Initialize(destination, new EndpointAddress("error"));

            var channelMessage = CreateChannelMessage();


            channel.Send(destination, channelMessage);

        }

        private ChannelMessage CreateChannelMessage()
        {
            var message = new TestMessage { Info = "Sending in pipes" };
            var stream = new MemoryStream();
            serializer.Serialize(message, stream);
            stream.Position = 0;
            return new ChannelMessage(stream);
        }
    }

    public class TestMessage
    {
        public string Info { get; set; }
    }
}
