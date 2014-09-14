using System;
using System.Collections.Generic;
using System.Linq;
using EzBus.Core.Builders;

namespace EzBus.Core
{
    public static class MessageFilterResolver
    {
        private static readonly List<Type> filterTypes = new List<Type>();

        static MessageFilterResolver()
        {
            LoadFilterTypes();
        }

        private static void LoadFilterTypes()
        {
            var scanner = new AssemblyScanner();
            var foundType = scanner.FindTypes<IMessageFilter>();
            filterTypes.Clear();
            filterTypes.AddRange(foundType);
        }

        public static IMessageFilter[] GetMessageFilters(IObjectFactory objectFactory)
        {
            return filterTypes.Select(x => (IMessageFilter)objectFactory.CreateInstance(x)).ToArray();
        }
    }
}