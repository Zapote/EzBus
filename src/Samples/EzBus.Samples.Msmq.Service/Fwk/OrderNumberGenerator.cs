using System;

namespace EzBus.Samples.Msmq.Service.Fwk
{
    public class OrderNumberGenerator : IOrderNumberGenerator
    {
        public int GenerateNumber(Guid orderId)
        {
            var orderNo = orderId.GetHashCode();
            if (orderNo < 0) return -orderNo;
            return orderNo;
        }
    }
}