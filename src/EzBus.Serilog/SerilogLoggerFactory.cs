namespace EzBus.Serilog
{
    public class SerilogLoggerFactory : Logging.LoggerFactory
    {
        public override Logging.ILogger CreateLogger(Logging.LogLevel lvl, string name)
        {
            return new SerilogLogger();
        }
    }
}
