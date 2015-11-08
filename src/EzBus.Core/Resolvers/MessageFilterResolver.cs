using System;
using System.Collections.Generic;
using System.Linq;
using EzBus.Core.Utils;

namespace EzBus.Core.Resolvers
{
    public static class MessageFilterResolver
    {
        private static readonly List<Type> filterTypes = new List<Type>();

        static MessageFilterResolver()
        {
            ResolveTypes();
        }

        private static void ResolveTypes()
        {
            var scanner = new AssemblyScanner();
            var foundType = scanner.FindTypes<IMessageFilter>();
            filterTypes.Clear();
            filterTypes.AddRange(foundType);
        }

        public static IMessageFilter[] GetMessageFilters(IObjectFactory objectFactory)
        {
            return filterTypes.Select(x => (IMessageFilter)objectFactory.GetInstance(x)).ToArray();
        }
    }

    public static class StartupTaskResolver
    {
        private static readonly List<Type> startupTaskTypes = new List<Type>();

        static StartupTaskResolver()
        {
            ResolveTypes();
        }

        private static void ResolveTypes()
        {
            var scanner = new AssemblyScanner();
            var foundType = scanner.FindTypes<IStartupTask>();
            startupTaskTypes.Clear();
            startupTaskTypes.AddRange(foundType);
        }

        public static IStartupTask[] GetStartupTasks()
        {
            return startupTaskTypes.Select(x => (IStartupTask)x.CreateInstance()).ToArray();
        }
    }
}