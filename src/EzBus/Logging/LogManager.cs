using System;

namespace EzBus.Logging
{
    public class LogManager
    {
        private static LoggerFactory loggerFactory;
        private static LogLevel logLevel = LogLevel.Info;

        public static ILogger GetLogger(string name)
        {
            return loggerFactory == null ? ConsoleLogger.Create(logLevel, name) : loggerFactory.CreateLogger(logLevel, name);
        }

        public static ILogger GetLogger(Type type)
        {
            return GetLogger(type.FullName);
        }

        public static ILogger GetLogger<T>()
        {
            return GetLogger(typeof(T));
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