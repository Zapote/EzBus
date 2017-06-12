using System.Reflection;
using log4net;

namespace EzBus.log4net
{
    public class log4netLoggerFactory : Logging.LoggerFactory
    {
        public override Logging.ILogger CreateLogger(Logging.LogLevel level, string name)
        {
            var repository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            var log = LogManager.GetLogger(repository.Name, name);
            return new log4netLogger(log);
        }
    }
}