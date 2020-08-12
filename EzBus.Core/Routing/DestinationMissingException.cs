using System;

namespace EzBus.Core.Routing
{
    public class DestinationMissingException : Exception
    {
        public DestinationMissingException(string message)
            : base(message)
        {

        }
    }
}