using System;

namespace EzBus.Samples.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            Bus.Start();
            Console.Read();
        }
    }
}
