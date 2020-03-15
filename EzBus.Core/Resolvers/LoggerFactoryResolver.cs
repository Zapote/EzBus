using EzBus.Logging;
using EzBus.Utils;

namespace EzBus.Core.Resolvers
{
    public class LoggerFactoryResolver
    {
        private static LoggerFactory instance;
        private static readonly object syncRoot = new object();

        public static LoggerFactory GetLoggerFactory()
        {
            lock (syncRoot)
            {
                if (instance != null) return instance;
                var loggerFactoryType = TypeResolver.GetType<LoggerFactory>();
                instance = (LoggerFactory)loggerFactoryType.CreateInstance();
            }

            return instance;
        }
    }
}