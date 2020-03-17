using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using EzBus.Logging;

[assembly: InternalsVisibleTo("EzBus.Core.Test")]
namespace EzBus.Core
{
    internal class Config : IConfig
    {
        public Config()
        {
            CreateAddress();
        }

        public string Address { get; private set; }
        public string ErrorAddress { get; private set; }
        public LogLevel LogLevel { get; private set; } = LogLevel.Info;
        public int NumberOfRetries { get; private set; } = 5;
        public int WorkerThreads { get; private set; } = 1;

        internal void SetAddress(string adr)
        {
            Address = adr;
            ErrorAddress = $"{adr}-error";
        }

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

        private void CreateAddress()
        {
            var assembly = Assembly.GetEntryAssembly();
            var address = assembly.GetName().Name.Replace(".","-").ToLower();
            Address = address;
            ErrorAddress = $"{address}-error";
        }
    }
}
