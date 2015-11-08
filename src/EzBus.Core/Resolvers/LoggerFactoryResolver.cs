using EzBus.Core.Utils;
using EzBus.Logging;

namespace EzBus.Core.Resolvers
{
    public class LoggerFactoryResolver
    {
        private static LoggerFactory instance;
        private static readonly object syncRoot = new object();

        public static LoggerFactory GetLoggerFactory()
        {
            if (instance != null) return instance;

            lock (syncRoot)
            {
                var loggerFactoryType = TypeResolver.Get<LoggerFactory>();
                instance = (LoggerFactory)loggerFactoryType.CreateInstance();
            }

            return instance;
        }
    }
}