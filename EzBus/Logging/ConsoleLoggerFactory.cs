namespace EzBus.Logging
{
    public class ConsoleLoggerFactory : LoggerFactory
    {
        public override ILogger CreateLogger(LogLevel lvl, string name)
        {
            return new ConsoleLogger(lvl, name);
        }
    }
}