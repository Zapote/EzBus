using System;
using EzBus.Logging;

namespace EzBus.Core.Logging
{
    public class TraceLogger : ILogger
    {
        private readonly LogLevel level;
        private readonly string name;

        public TraceLogger(LogLevel level, string name)
        {
            this.level = level;
            this.name = name;
        }

        public bool IsVerboseEnabled => level <= LogLevel.Verbose && LoggingEnabled();
        public bool IsDebugEnabled => level <= LogLevel.Debug && LoggingEnabled();
        public bool IsInfoEnabled => level <= LogLevel.Info && LoggingEnabled();
        public bool IsWarnEnabled => level <= LogLevel.Warn && LoggingEnabled();
        public bool IsErrorEnabled => level <= LogLevel.Error && LoggingEnabled();
        public bool IsFatalEnabled => level <= LogLevel.Fatal && LoggingEnabled();

        private bool LoggingEnabled()
        {
            return level != LogLevel.Off;
        }

        public void Verbose(object message)
        {
            if (!IsVerboseEnabled) return;
            WriteLog(message, LogLevel.Verbose);
        }

        public void Debug(object message)
        {
            if (!IsDebugEnabled) return;
            WriteLog(message, LogLevel.Debug);
        }

        public void Info(object message)
        {
            if (!IsInfoEnabled) return;
            WriteLog(message, LogLevel.Info, ConsoleColor.Blue);
        }

        public void Warn(object message)
        {
            if (!IsWarnEnabled) return;
            WriteLog(message, LogLevel.Warn, ConsoleColor.Yellow);
        }

        public void Error(object message)
        {
            if (!IsErrorEnabled) return;
            WriteLog(message, LogLevel.Error, ConsoleColor.Red);
        }

        public void Fatal(object message)
        {
            if (!IsFatalEnabled) return;
            WriteLog(message, LogLevel.Fatal, ConsoleColor.Red);
        }

        public void Verbose(object message, Exception t)
        {
            WriteLog($"{message} {t}", LogLevel.Verbose);
        }

        public void Debug(object message, Exception t)
        {
            WriteLog($"{message} {t}", LogLevel.Debug);
        }

        public void Info(object message, Exception t)
        {
            WriteLog($"{message} {t}", LogLevel.Info, ConsoleColor.Blue);
        }

        public void Warn(object message, Exception t)
        {
            WriteLog($"{message} {t}", LogLevel.Warn, ConsoleColor.Yellow);
        }

        public void Error(object message, Exception t)
        {
            WriteLog($"{message} {t}", LogLevel.Error, ConsoleColor.Red);
        }

        public void Fatal(object message, Exception t)
        {
            WriteLog($"{message} {t}", LogLevel.Fatal, ConsoleColor.Red);
        }

        private void WriteLog(object message, LogLevel logLevel, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.ForegroundColor = color;
           // Trace.WriteLine($"{DateTime.Now} [{System.Threading.Thread.CurrentThread.ManagedThreadId}] {logLevel} {name}: {message}");
            Console.ResetColor();
        }
    }
}