namespace EzBus
{
    public interface ISendingChannel
    {
        void Send(EndpointAddress dest, ChannelMessage cm);
    }
}