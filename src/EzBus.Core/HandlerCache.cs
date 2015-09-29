using System;
using System.Collections.Generic;
using System.Linq;
using EzBus.Core.Utils;
using EzBus.Logging;

namespace EzBus.Core
{
    public class HandlerCache
    {
        private static readonly ILogger log = HostLogManager.GetLogger(typeof(HandlerCache));
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

            log.VerboseFormat("Handler '{0}' for message '{1}' added to cache", handlerType.FullName, messageType.FullName);
        }

        public IEnumerable<HandlerInfo> GetHandlerInfo(string messageTypeName)
        {
            return handlers.Where(x => x.Key == messageTypeName).Select(x => x.Value);
        }

        private static Type GetMessageType(Type handlerType)
        {
            var handlerInterface = handlerType.GetInterface(typeof(IHandle<>).Name);
            return handlerInterface.GetGenericArguments()[0];
        }

        public void Prime()
        {
            var scanner = new AssemblyScanner();
            var handlerTypes = scanner.FindTypes(typeof(IHandle<>));

            foreach (var handlerType in handlerTypes)
            {
                Add(handlerType);
            }
        }

        public bool HasCustomHandlers()
        {
            if (handlers.Count == 0)
            {
                return true;
            }

            return handlers.Count == 1 && handlers[0].Value.HandlerType == typeof(SubscriptionMessageHandler);
        }

        public int NumberOfEntries
        {
            get { return handlers.Count; }
        }
    }
}