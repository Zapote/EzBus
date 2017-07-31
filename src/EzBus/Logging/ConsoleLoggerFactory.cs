namespace EzBus.Logging
{
    public class ConsoleLoggerFactory : LoggerFactory
    {
        public override ILogger CreateLogger(LogLevel level, string name)
        {
            return new ConsoleLogger(level, name);
        }
    }
}