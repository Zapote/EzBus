using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using EzBus.Logging;

[assembly: InternalsVisibleTo("EzBus.Core.Test")]
namespace EzBus.Core
{
    internal class BusConfig : IBusConfig
    {
        public BusConfig(string addr)
        {
            Address = addr;
        }

        public string Address { get; private set; }
        public string ErrorAddress => $"{Address}-error";
        public LogLevel LogLevel { get; private set; } = LogLevel.Info;
        public int NumberOfRetries { get; private set; } = 5;
        public int WorkerThreads { get; private set; } = 1;

        internal void SetLogLevel(LogLevel l)
        {
            LogLevel = l;
        }

        internal void SetNumberOfRetries(int n)
        {
            NumberOfRetries = n;
        }

        internal void SetNumberOfWorkerThreads(int n)
        {
            WorkerThreads = n;
        }
    }
}
