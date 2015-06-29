using System;
using System.Diagnostics;
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

        public bool IsVerboseEnabled { get { return level <= LogLevel.Verbose && LoggingOn(); } }
        public bool IsDebugEnabled { get { return level <= LogLevel.Debug && LoggingOn(); } }
        public bool IsInfoEnabled { get { return level <= LogLevel.Info && LoggingOn(); } }
        public bool IsWarnEnabled { get { return level <= LogLevel.Warn && LoggingOn(); } }
        public bool IsErrorEnabled { get { return level <= LogLevel.Error && LoggingOn(); } }
        public bool IsFatalEnabled { get { return level <= LogLevel.Fatal && LoggingOn(); } }

        private bool LoggingOn()
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
            VerboseFormat("{0} {1}", message, t);
        }

        public void Debug(object message, Exception t)
        {
            DebugFormat("{0} {1}", message, t);
        }

        public void Info(object message, Exception t)
        {
            InfoFormat("{0} {1}", message, t);
        }

        public void Warn(object message, Exception t)
        {
            WarnFormat("{0} {1}", message, t);
        }

        public void Error(object message, Exception t)
        {
            ErrorFormat("{0} {1}", message, t);
        }

        public void Fatal(object message, Exception t)
        {
            FatalFormat("{0} {1}", message, t);
        }

        public void VerboseFormat(string format, params object[] args)
        {
            Verbose(string.Format(format, args));
        }

        public void DebugFormat(string format, params object[] args)
        {
            Debug(string.Format(format, args));
        }

        public void InfoFormat(string format, params object[] args)
        {
            Info(string.Format(format, args));
        }

        public void WarnFormat(string format, params object[] args)
        {
            Warn(string.Format(format, args));
        }

        public void ErrorFormat(string format, params object[] args)
        {
            Error(string.Format(format, args));
        }

        public void FatalFormat(string format, params object[] args)
        {
            Fatal(string.Format(format, args));
        }

        private void WriteLog(object message, LogLevel logLevel, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.ForegroundColor = color;
            Trace.WriteLine(string.Format("{0} [{1}] {2} {3}: {4}", DateTime.Now, System.Threading.Thread.CurrentThread.ManagedThreadId, logLevel, name, message));
            Console.ResetColor();
        }
    }
}