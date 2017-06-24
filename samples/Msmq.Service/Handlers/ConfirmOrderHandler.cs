using System;
using EzBus;
using Msmq.Service.Messages;

namespace Msmq.Service.Handlers
{
    public class ConfirmOrderHandler : IHandle<ConfirmOrder>
    {
        public void Handle(ConfirmOrder message)
        {
            Console.WriteLine($"Order '{message.OrderId}' confirmed!");
            Bus.Publish(new OrderConfirmed
            {
                OrderId = message.OrderId,
                ConfirmationDate = DateTime.Now
            });
        }
    }
}