using System;

namespace EzBus.Logging
{
    public interface ILogger
    {
        bool IsVerboseEnabled { get; }
        bool IsDebugEnabled { get; }
        bool IsInfoEnabled { get; }
        bool IsWarnEnabled { get; }
        bool IsErrorEnabled { get; }
        bool IsFatalEnabled { get; }

        void Verbose(object message);
        void Debug(object message);
        void Info(object message);
        void Warn(object message);
        void Error(object message);
        void Fatal(object message);

        void Verbose(object message, Exception t);
        void Debug(object message, Exception t);
        void Info(object message, Exception t);
        void Warn(object message, Exception t);
        void Error(object message, Exception t);
        void Fatal(object message, Exception t);

        void VerboseFormat(string format, params object[] args);
        void DebugFormat(string format, params object[] args);
        void InfoFormat(string format, params object[] args);
        void WarnFormat(string format, params object[] args);
        void ErrorFormat(string format, params object[] args);
        void FatalFormat(string format, params object[] args);
    }
}