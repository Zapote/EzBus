using System;
using EzBus.Samples.Messages.Commands;
using EzBus.Samples.Messages.Events;
using EzBus.Samples.Msmq.Service.Fwk;
using log4net;

namespace EzBus.Samples.Msmq.Service.Handlers
{
    public class CreateOrderHandler : IHandle<CreateOrder>
    {
        private readonly ILog log = LogManager.GetLogger(typeof(CreateOrderHandler));
        private readonly IOrderNumberGenerator orderNumberGenerator;

        public CreateOrderHandler(IOrderNumberGenerator orderNumberGenerator)
        {
            if (orderNumberGenerator == null) throw new ArgumentNullException(nameof(orderNumberGenerator));
            this.orderNumberGenerator = orderNumberGenerator;
        }

        public void Handle(CreateOrder message)
        {
            log.Debug($"Generating number for order: {message.OrderId}");
            var orderNumber = orderNumberGenerator.GenerateNumber(message.OrderId);
            Bus.Publish(new OrderCreated(message.OrderId, orderNumber));
        }
    }
}