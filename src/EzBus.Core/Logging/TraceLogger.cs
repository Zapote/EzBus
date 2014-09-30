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

        public bool IsDebugEnabled { get { return level >= LogLevel.Debug; } }
        public bool IsInfoEnabled { get { return level >= LogLevel.Info; } }
        public bool IsWarnEnabled { get { return level >= LogLevel.Warn; } }
        public bool IsErrorEnabled { get { return level >= LogLevel.Error; } }
        public bool IsFatalEnabled { get { return level >= LogLevel.Fatal; } }

        public void Debug(object message)
        {
            if (!IsDebugEnabled) return;
            WriteLog(message);
        }

        public void Info(object message)
        {
            if (!IsInfoEnabled) return;
            WriteLog(message);
        }

        public void Warn(object message)
        {
            if (!IsWarnEnabled) return;
            WriteLog(message);
        }

        public void Error(object message)
        {
            if (!IsErrorEnabled) return;
            WriteLog(message);
        }

        public void Fatal(object message)
        {
            if (!IsFatalEnabled) return;
            WriteLog(message);
        }

        public void Debug(object message, Exception t)
        {
            throw new NotImplementedException();
        }

        public void Info(object message, Exception t)
        {
            throw new NotImplementedException();
        }

        public void Warn(object message, Exception t)
        {
            throw new NotImplementedException();
        }

        public void Error(object message, Exception t)
        {
            ErrorFormat("{0} {1}", message, t);
        }

        public void Fatal(object message, Exception t)
        {
            throw new NotImplementedException();
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

        private void WriteLog(object message)
        {
            Trace.WriteLine(string.Format("{0} [{1}] {2} {3}\t{4}", DateTime.Now, System.Threading.Thread.CurrentThread.ManagedThreadId, level, name, message));
        }
    }
}