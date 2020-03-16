using System;

namespace EzBus
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public BasicMessage Message { get; set; }
    }
}