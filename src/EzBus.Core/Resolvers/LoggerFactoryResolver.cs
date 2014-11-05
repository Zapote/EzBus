using System;
using System.Linq;
using EzBus.Core.Logging;
using EzBus.Logging;

namespace EzBus.Core.Resolvers
{
    internal class LoggerFactoryResolver
    {
        private static Type loggerFactoryType;

        static LoggerFactoryResolver()
        {
            ResolveTypes();
        }

        private static void ResolveTypes()
        {
            var scanner = new AssemblyScanner();
            var foundTypes = scanner.FindTypes<LoggerFactory>();
            loggerFactoryType = foundTypes.LastOrDefault(x => !x.IsLocal()) ?? typeof(TraceLoggerFactory);
        }

        public static LoggerFactory GetLoggerFactory()
        {
            return loggerFactoryType.CreateInstance() as LoggerFactory;
        }
    }
}