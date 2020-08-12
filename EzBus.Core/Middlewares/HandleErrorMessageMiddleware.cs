using System;
using System.Threading.Tasks;

namespace EzBus.Core.Middlewares
{
    internal class HandleErrorMessageMiddleware : IPreMiddleware
    {
        private readonly IBroker broker;
        private readonly IAddressConfig addressConfig;
        private BasicMessage bm;

        public HandleErrorMessageMiddleware(IBroker broker, IAddressConfig addressConfig)
        {
            this.broker = broker ?? throw new ArgumentNullException(nameof(broker));
            this.addressConfig = addressConfig ?? throw new ArgumentNullException(nameof(addressConfig));
        }

        public async Task Invoke(MiddlewareContext context, Func<Task> next)
        {
            bm = context.BasicMessage;
            await next();
        }

        public async Task OnError(Exception ex)
        {
            if (bm == null) throw new Exception("Message is null!", ex);
            bm.BodyStream.Position = 0;

            var level = 0;

            while (ex != null)
            {
                var headerName = $"EzBus.ErrorMessage L{level}";
                var value = $"{DateTime.UtcNow}: {ex.Message}";
                bm.AddHeader(headerName, value);
                ex = ex.InnerException;
                level++;
            }

            await broker.Send(addressConfig.ErrorAddress, bm);
        }
    }
}