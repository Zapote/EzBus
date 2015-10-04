namespace EzBus.Msmq
{
    public class MsmqSendingChannel : ISendingChannel
    {
        public void Send(EndpointAddress destination, ChannelMessage channelMessage)
        {
            MsmqUtilities.WriteMessage(destination, channelMessage);
        }
    }
}