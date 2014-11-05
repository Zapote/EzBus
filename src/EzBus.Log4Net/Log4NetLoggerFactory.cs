using EzBus.Logging;
using log4net;

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