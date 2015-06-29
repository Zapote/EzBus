using System;
using EzBus.Logging;
using log4net;

namespace EzBus.Log4Net
{
    public class Log4NetLogger : ILogger
    {
        private readonly ILog log;

        public Log4NetLogger(ILog log)
        {
            if (log == null) throw new ArgumentNullException("log");
            this.log = log;
        }

        public bool IsVerboseEnabled { get { return false; } }
        public bool IsDebugEnabled { get { return log.IsDebugEnabled; } }
        public bool IsInfoEnabled { get { return log.IsInfoEnabled; } }
        public bool IsWarnEnabled { get { return log.IsWarnEnabled; } }
        public bool IsErrorEnabled { get { return log.IsErrorEnabled; } }
        public bool IsFatalEnabled { get { return log.IsFatalEnabled; } }

        public void Verbose(object message)
        {
        }

        public void Debug(object message)
        {
            log.Debug(message);
        }

        public void Info(object message)
        {
            log.Info(message);
        }

        public void Warn(object message)
        {
            log.Warn(message);
        }

        public void Error(object message)
        {
            log.Error(message);
        }

        public void Fatal(object message)
        {
            log.Fatal(message);
        }

        public void Verbose(object message, Exception t) { }

        public void Debug(object message, Exception t)
        {
            log.Debug(message, t);
        }

        public void Info(object message, Exception t)
        {
            log.Info(message, t);
        }

        public void Warn(object message, Exception t)
        {
            log.Warn(message, t);
        }

        public void Error(object message, Exception t)
        {
            log.Error(message, t);
        }

        public void Fatal(object message, Exception t)
        {
            log.Fatal(message, t);
        }

        public void VerboseFormat(string format, params object[] args) { }

        public void DebugFormat(string format, params object[] args)
        {
            log.DebugFormat(format, args);
        }

        public void InfoFormat(string format, params object[] args)
        {
            log.InfoFormat(format, args);
        }

        public void WarnFormat(string format, params object[] args)
        {
            log.WarnFormat(format, args);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            log.ErrorFormat(format, args);
        }

        public void FatalFormat(string format, params object[] args)
        {
            log.FatalFormat(format, args);
        }
    }
}
