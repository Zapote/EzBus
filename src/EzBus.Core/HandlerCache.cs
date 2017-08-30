using System;
using System.Collections.Generic;
using System.Linq;
using EzBus.Logging;
using EzBus.Utils;
using EzBus.Core.Utils;

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
            log.Verbose($"Handler '{handlerType.FullName}' for message '{messageType.FullName}' added to cache");
        }

        public IEnumerable<HandlerInfo> GetHandlerInfo(string messageTypeName)
        {
            var nameParts = messageTypeName.Split('.');
            var className = nameParts.Last();
            var result = handlers.Where(x => x.Key == messageTypeName).ToList();

            if (!result.Any())
            {
                result = handlers.Where(x => x.Key.EndsWith(className)).ToList();
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
                log.Debug($"Found handler {handlerType.Name}");
                Add(handlerType);
            }
        }

        public bool HasCustomHandlers()
        {
            return handlers.Any(x => !x.Value.HandlerType.IsLocal());
        }

        public int NumberOfEntries => handlers.Count;
    }
}