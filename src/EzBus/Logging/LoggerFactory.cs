namespace EzBus.Logging
{
    public abstract class LoggerFactory
    {
        public abstract ILogger CreateLogger(LogLevel level, string name);
    }
}