using System;
using System.Collections.Generic;
using System.Linq;

namespace EzBus.Core
{
    public class HandlerCache
    {
        private readonly List<KeyValuePair<string, HandlerInfo>> handlers = new List<KeyValuePair<string, HandlerInfo>>();

        public void Add(Type handlerType)
        {
            var messageType = GetMessageType(handlerType);
            handlers.Add(new KeyValuePair<string, HandlerInfo>(messageType.FullName,
                new HandlerInfo
                {
                    HandlerType = handlerType,
                    MessageType = messageType
                }));
        }

        public IEnumerable<HandlerInfo> GetHandlerInfo(string messageTypeName)
        {
            return handlers.Where(x => x.Key == messageTypeName).Select(x => x.Value);
        }

        private static Type GetMessageType(Type handlerType)
        {
            var handlerInterface = handlerType.GetInterface(typeof(IMessageHandler<>).Name);
            return handlerInterface.GetGenericArguments()[0];
        }
    }
}