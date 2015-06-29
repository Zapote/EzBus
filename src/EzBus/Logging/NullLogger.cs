using System;

namespace EzBus.Logging
{
    public class NullLogger : ILogger
    {
        public static ILogger Create()
        {
            return new NullLogger();
        }

        private NullLogger() { }

        public bool IsVerboseEnabled { get; private set; }
        public bool IsDebugEnabled { get; private set; }
        public bool IsInfoEnabled { get; private set; }
        public bool IsWarnEnabled { get; private set; }
        public bool IsErrorEnabled { get; private set; }
        public bool IsFatalEnabled { get; private set; }

        public void Verbose(object message)
        {

        }

        public void Debug(object message)
        {

        }

        public void Info(object message)
        {
        }

        public void Warn(object message)
        {
        }

        public void Error(object message)
        {
        }

        public void Fatal(object message)
        {
        }

        public void Verbose(object message, Exception t)
        {
        }

        public void Debug(object message, Exception t)
        {
        }

        public void Info(object message, Exception t)
        {
        }

        public void Warn(object message, Exception t)
        {
        }

        public void Error(object message, Exception t)
        {
        }

        public void Fatal(object message, Exception t)
        {
        }

        public void VerboseFormat(string format, params object[] args)
        {
        }

        public void DebugFormat(string format, params object[] args)
        {
        }

        public void InfoFormat(string format, params object[] args)
        {
        }

        public void WarnFormat(string format, params object[] args)
        {
        }

        public void ErrorFormat(string format, params object[] args)
        {
        }

        public void FatalFormat(string format, params object[] args)
        {
        }
    }
}