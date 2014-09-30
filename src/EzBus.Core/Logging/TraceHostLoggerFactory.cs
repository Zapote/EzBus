using System.Diagnostics;
using EzBus.Logging;

namespace EzBus.Core.Logging
{
    public class TraceHostLoggerFactory : LoggerFactory
    {
        public TraceHostLoggerFactory()
        {
            Trace.Listeners.Add(new ConsoleTraceListener());
        }

        public override ILogger CreateLogger(LogLevel level, string name)
        {
            return new TraceLogger(LogLevel.Debug, name);
        }
    }
}