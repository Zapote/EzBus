namespace EzBus
{
    public class MiddlewareContext
    {
        public MiddlewareContext(ChannelMessage channelMessage)
        {
            ChannelMessage = channelMessage;
        }

        public ChannelMessage ChannelMessage { get; private set; }
        public object Message { get; set; }
    }
}