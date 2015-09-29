using System;
using System.Linq;
using EzBus.Core.Logging;
using EzBus.Core.Utils;
using EzBus.Logging;

namespace EzBus.Core.Resolvers
{
    public class LoggerFactoryResolver : ResolverBase<LoggerFactory, TraceLoggerFactory>
    {
        private static readonly LoggerFactoryResolver instance = new LoggerFactoryResolver();

        public static LoggerFactory GetLoggerFactory()
        {
            return instance.GetInstance();
        }
    }
}