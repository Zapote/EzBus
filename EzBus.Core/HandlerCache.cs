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
            foreach (var messageType in GetMessageTypes(handlerType))
            {
                var handlerInfo = new HandlerInfo(handlerType, messageType);
                handlers.Add(new KeyValuePair<string, HandlerInfo>(messageType.FullName, handlerInfo));
                log.Verbose($"Handler '{handlerType.FullName}' for message '{messageType.FullName}' added to cache");
            }
        }

        public IEnumerable<HandlerInfo> GetHandlerInfo(string messageFullName)
        {
            var className = GetClassName(messageFullName);
            var result = handlers.Where(x => x.Key == messageFullName).ToList();

            if (!result.Any())
            {
                result = handlers.Where(x => GetClassName(x.Key) == className).ToList();
            }

            return result.Select(x => x.Value);
        }

        private static string GetClassName(string messageFullName)
        {
            var parts = messageFullName.Split('.');
            return parts.Last();
        }

        private static IEnumerable<Type> GetMessageTypes(Type handlerType)
        {
            var handlerInterface = handlerType.GetInterfaces().Where( x => x.Name == typeof(IHandle<>).Name);
            return handlerInterface.Select(i => i.GetGenericArguments()[0]).ToArray();
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