using System;
using log4net;
using log4net.Core;

namespace EzBus.Log4Net
{
    public class Log4NetLogger : Logging.ILogger
    {
        private readonly ILog log;

        public Log4NetLogger(ILog log)
        {
            if (log == null) throw new ArgumentNullException(nameof(log));
            this.log = log;
        }

        public bool IsVerboseEnabled => log.Logger.IsEnabledFor(Level.Verbose);
        public bool IsDebugEnabled => log.IsDebugEnabled;
        public bool IsInfoEnabled => log.IsInfoEnabled;
        public bool IsWarnEnabled => log.IsWarnEnabled;
        public bool IsErrorEnabled => log.IsErrorEnabled;
        public bool IsFatalEnabled => log.IsFatalEnabled;

        public void Verbose(object message)
        {
            log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, Level.Verbose, message, null);
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

        public void Verbose(object message, Exception t)
        {
            log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, Level.Verbose, message, t);
        }

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
    }
}
