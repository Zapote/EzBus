using System.Reflection;
using log4net;

namespace EzBus.log4net
{
    public class Log4NetLoggerFactory : Logging.LoggerFactory
    {
        public override Logging.ILogger CreateLogger(Logging.LogLevel lvl, string name)
        {
            var ass = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
            var repo = LogManager.GetRepository(ass);
            var log = LogManager.GetLogger(repo.Name, name);
            return new log4netLogger(log);
        }
    }
}