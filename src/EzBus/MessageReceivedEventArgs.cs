using System;

namespace EzBus
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public ChannelMessage Message { get; set; }
    }
}