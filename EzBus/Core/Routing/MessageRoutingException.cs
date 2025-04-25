using System;

namespace EzBus.Core.Routing
{
    public class MessageRoutingException : Exception
    {
        public MessageRoutingException(string message)
            : base(message)
        {

        }
    }
}