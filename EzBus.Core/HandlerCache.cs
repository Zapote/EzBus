using System;
using System.Collections.Generic;
using System.Linq;
using EzBus.Utils;
using EzBus.Core.Utils;
using Microsoft.Extensions.Logging;

namespace EzBus.Core
{
    public class HandlerCache : IHandlerCache
    {
        private readonly IDictionary<string, List<HandlerInfo>> handlers = new Dictionary<string, List<HandlerInfo>>();
        private readonly ILogger<HandlerCache> logger;
        private readonly IAssemblyScanner assemblyScanner;

        public HandlerCache(ILogger<HandlerCache> logger, IAssemblyScanner assemblyScanner)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.assemblyScanner = assemblyScanner ?? throw new ArgumentNullException(nameof(assemblyScanner));
        }

        public void Add(Type handlerType)
        {
            foreach (var messageType in GetMessageTypes(handlerType))
            {
                var handlerInfo = new HandlerInfo(handlerType, messageType);
                var key = messageType.FullName;
                if (!handlers.ContainsKey(key))
                {
                    handlers[key] = new List<HandlerInfo>();
                }
                handlers[key].Add(handlerInfo);
                logger.LogDebug($"Handler '{handlerType.FullName}' for message '{messageType.FullName}' added to cache");
            }
        }

        public HandlerInfo[] GetHandlerInfo(string messageFullName)
        {
            if (handlers.ContainsKey(messageFullName))
            {
                return handlers[messageFullName].ToArray();
            }

            var className = GetClassName(messageFullName);
            var result = handlers.FirstOrDefault(x => GetClassName(x.Key) == className).Value;
            if (result == null) return new HandlerInfo[0];
            return result.ToArray();
        }

        private static string GetClassName(string messageFullName)
        {
            var parts = messageFullName.Split('.');
            return parts.Last();
        }

        private static IEnumerable<Type> GetMessageTypes(Type handlerType)
        {
            var handlerInterface = handlerType.GetInterfaces().Where(x => x.Name == typeof(IHandle<>).Name);
            return handlerInterface.Select(i => i.GetGenericArguments()[0]).ToArray();
        }

        private void Clear()
        {
            handlers.Clear();
        }

        public void Prime()
        {
            var handlerTypes = assemblyScanner.FindTypes(typeof(IHandle<>));

            Clear();

            foreach (var handlerType in handlerTypes)
            {
                logger.LogDebug($"Found handler {handlerType.Name}");
                Add(handlerType);
            }
        }

        public int NumberOfEntries => handlers.Sum(x => x.Value.Count);
    }
}