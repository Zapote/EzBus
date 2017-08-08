using System;
using EzBus.Msmq;
using Msmq.Client.Messages;

namespace Msmq.Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Msmq.Client";

            Bus.Configure().UseMsmq();

            var orderId = Guid.NewGuid();
            Bus.Send("msmq.service", new ConfirmOrder(orderId));

            Console.WriteLine($"Order confirmation requested. {orderId}");

            Console.Read();
        }
    }
}
