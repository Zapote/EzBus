using System;
using System.Collections.Generic;
using System.Linq;
using EzBus.Core.Utils;
using EzBus.Logging;

namespace EzBus.Core
{
    public class HandlerCache : IHandlerCache
    {
        private static readonly ILogger log = LogManager.GetLogger(typeof(HandlerCache));
        private readonly List<KeyValuePair<string, HandlerInfo>> handlers = new List<KeyValuePair<string, HandlerInfo>>();

        public void Add(Type handlerType)
        {
            var messageType = GetMessageType(handlerType);
            var handlerInfo = new HandlerInfo(handlerType, messageType);
            handlers.Add(new KeyValuePair<string, HandlerInfo>(messageType.FullName, handlerInfo));
            log.VerboseFormat("Handler '{0}' for message '{1}' added to cache", handlerType.FullName, messageType.FullName);
        }

        public IEnumerable<HandlerInfo> GetHandlerInfo(string messageTypeName)
        {
            var className = messageTypeName.Split('.').Last();
            var result = handlers.Where(x => x.Key == messageTypeName);

            if (!result.Any())
            {
                result = handlers.Where(x => x.Key.EndsWith(className));
            }

            return result.Select(x => x.Value);
        }

        private static Type GetMessageType(Type handlerType)
        {
            var handlerInterface = handlerType.GetInterface(typeof(IHandle<>).Name);
            return handlerInterface.GetGenericArguments()[0];
        }

        private void Clear()
        {
            handlers.Clear();
        }

        public void Prime()
        {
            var scanner = new AssemblyScanner();
            var handlerTypes = scanner.FindTypes(typeof(IHandle<>));

            Clear();

            foreach (var handlerType in handlerTypes)
            {
                Add(handlerType);
            }
        }

        public bool HasCustomHandlers()
        {
            return handlers.Count(x => !x.Value.HandlerType.IsLocal()) == 0;
        }

        public int NumberOfEntries => handlers.Count;
    }
}