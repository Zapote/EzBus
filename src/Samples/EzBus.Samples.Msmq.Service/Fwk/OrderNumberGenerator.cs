using System;

namespace EzBus.Samples.Msmq.Service.Fwk
{
    public class OrderNumberGenerator : IOrderNumberGenerator
    {
        public Guid Id { get; set; }

        public OrderNumberGenerator()
        {
            Id = Guid.NewGuid();
        }

        public int GenerateNumber(Guid orderId)
        {
            var orderNo = orderId.GetHashCode();
            if (orderNo < 0) return -orderNo;
            return orderNo;
        }
    }
}