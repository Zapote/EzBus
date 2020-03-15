namespace EzBus.Logging
{
    public abstract class LoggerFactory
    {
        public abstract ILogger CreateLogger(LogLevel lvl, string name);
    }
}