using System;

namespace EzBus.Samples.Msmq.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "EzBus.Samples.Msmq.Service";
            log4net.Config.XmlConfigurator.Configure();
            Bus.Start();
            Console.Read();
        }
    }
}
