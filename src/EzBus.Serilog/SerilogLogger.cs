using System;
using Serilog;
using Serilog.Events;

namespace EzBus.Serilog
{
    public class SerilogLogger : Logging.ILogger
    {
        public bool IsVerboseEnabled => Log.IsEnabled(LogEventLevel.Verbose);

        public bool IsDebugEnabled => Log.IsEnabled(LogEventLevel.Verbose);

        public bool IsInfoEnabled => Log.IsEnabled(LogEventLevel.Information);

        public bool IsWarnEnabled => Log.IsEnabled(LogEventLevel.Warning);

        public bool IsErrorEnabled => Log.IsEnabled(LogEventLevel.Error);

        public bool IsFatalEnabled => Log.IsEnabled(LogEventLevel.Fatal);

        public void Debug(object message)
        {
            Log.Debug(message?.ToString());
        }

        public void Debug(object message, Exception t)
        {
            Log.Debug(t, "{Message}", message);
        }

        public void Error(object message)
        {
            Log.Error(message?.ToString());
        }

        public void Error(object message, Exception t)
        {
            Log.Error(t, "{Message}", message);
        }

        public void Fatal(object message)
        {
            Log.Fatal(message?.ToString());
        }

        public void Fatal(object message, Exception t)
        {
            Log.Fatal(t, "{Message}", message);
        }

        public void Info(object message)
        {
            Log.Information(message?.ToString());
        }

        public void Info(object message, Exception t)
        {
            Log.Information(t, "{Message}", message);
        }

        public void Verbose(object message)
        {
            Log.Verbose(message?.ToString());
        }

        public void Verbose(object message, Exception t)
        {
            Log.Verbose(t, "{Message}", message);
        }

        public void Warn(object message)
        {
            Log.Warning(message?.ToString());
        }

        public void Warn(object message, Exception t)
        {
            Log.Warning(t, "{Message}", message);
        }
    }
}
