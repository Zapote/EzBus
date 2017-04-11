using System;
using EzBus.Samples.Msmq.Service.Fwk;
using log4net;

namespace EzBus.Samples.Msmq.Service.Middlewares
{
    public class UnitOfWorkMiddleware : IMiddleware
    {
        private readonly IOrderNumberGenerator orderNumberGenerator;
        private readonly ILog log = LogManager.GetLogger(typeof(UnitOfWorkMiddleware));
        private readonly int id;

        public UnitOfWorkMiddleware(IOrderNumberGenerator orderNumberGenerator)
        {
            if (orderNumberGenerator == null) throw new ArgumentNullException(nameof(orderNumberGenerator));
            this.orderNumberGenerator = orderNumberGenerator;
            var rand = new Random(Guid.NewGuid().GetHashCode());
            id = rand.Next(10000, 20000);
        }

        public void Invoke(MiddlewareContext context, Action next)
        {
            log.Info($"Start UOW {id}");
            next();
            log.Info($"End UOW {id}");
        }

        public void OnError(Exception ex)
        {

        }
    }
}