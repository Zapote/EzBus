using System;

namespace EzBus.Logging
{
    public class LogManager
    {
        private static LoggerFactory loggerFactory;
        private static LogLevel logLevel;

        public static ILogger GetLogger(string name)
        {
            return loggerFactory == null ? NullLogger.Create() : loggerFactory.CreateLogger(logLevel, name);
        }

        public static ILogger GetLogger(Type type)
        {
            return GetLogger(type.Name);
        }

        public static ILogger GetLogger<T>()
        {
            return GetLogger(typeof(T).Name);
        }

        public static void SetLogLevel(LogLevel level)
        {
            logLevel = level;
        }

        public static void Configure(LoggerFactory factory, LogLevel level)
        {
            loggerFactory = factory;
            logLevel = level;
        }
    }
}