using EzBus.Logging;
using LogManager = log4net.LogManager;

namespace EzBus.Log4Net
{
    public class Log4NetLoggerFactory : LoggerFactory
    {
        public override ILogger CreateLogger(LogLevel level, string name)
        {
            return new Log4NetLogger(LogManager.GetLogger(name));
        }
    }
}