using System;
using Serilog;
using Serilog.Events;

namespace EzBus.Serilog
{
    public class SerilogLogger : Logging.ILogger
    {
        private ILogger logger;

        public SerilogLogger()
        {
            var log = new LoggerConfiguration()
                //.WriteTo.Console()
                // .WriteTo.File("log.txt")
                .CreateLogger();

        }

        public bool IsVerboseEnabled => logger.IsEnabled(LogEventLevel.Verbose);

        public bool IsDebugEnabled => logger.IsEnabled(LogEventLevel.Verbose);

        public bool IsInfoEnabled => logger.IsEnabled(LogEventLevel.Information);

        public bool IsWarnEnabled => logger.IsEnabled(LogEventLevel.Warning);

        public bool IsErrorEnabled => logger.IsEnabled(LogEventLevel.Error);

        public bool IsFatalEnabled => logger.IsEnabled(LogEventLevel.Fatal);

        public void Debug(object message)
        {
            logger.Debug(message?.ToString());
        }

        public void Debug(object message, Exception t)
        {
            logger.Debug(t, "{Message}", message);
        }

        public void Error(object message)
        {
            logger.Error(message?.ToString());
        }

        public void Error(object message, Exception t)
        {
            logger.Error(t, "{Message}", message);
        }

        public void Fatal(object message)
        {
            logger.Fatal(message?.ToString());
        }

        public void Fatal(object message, Exception t)
        {
            logger.Fatal(t, "{Message}", message);
        }

        public void Info(object message)
        {
            logger.Information(message?.ToString());
        }

        public void Info(object message, Exception t)
        {
            logger.Information(t, "{Message}", message);
        }

        public void Verbose(object message)
        {
            logger.Verbose(message?.ToString());
        }

        public void Verbose(object message, Exception t)
        {
            logger.Verbose(t, "{Message}", message);
        }

        public void Warn(object message)
        {
            logger.Warning(message?.ToString());
        }

        public void Warn(object message, Exception t)
        {
            logger.Warning(t, "{Message}", message);
        }
    }
}
