using System.Diagnostics;
using EzBus.Logging;

namespace EzBus.Core.Logging
{
    public class TraceLoggerFactory : LoggerFactory
    {
        public TraceLoggerFactory()
        {
            //Trace.Listeners.Add(new ConsoleTraceListener());
        }

        public override ILogger CreateLogger(LogLevel level, string name)
        {
            return new ConsoleLogger(LogLevel.Debug, name);
        }
    }
}