using System;

namespace EzBus.Core
{
    public class HandlerInfo
    {
        public HandlerInfo(Type handlerType, Type messageType)
        {
            HandlerType = handlerType;
            MessageType = messageType;
        }

        public Type HandlerType { get; private set; }
        public Type MessageType { get; private set; }
    }
}