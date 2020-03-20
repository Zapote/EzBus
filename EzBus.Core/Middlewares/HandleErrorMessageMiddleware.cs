using System;

namespace EzBus.Core.Middlewares
{
    internal class HandleErrorMessageMiddleware : IPreMiddleware
    {
        private readonly IBroker broker;
        private readonly IAddressConfig addressConfig;
        private BasicMessage basicMessage;

        public HandleErrorMessageMiddleware(IBroker broker, IAddressConfig addressConfig)
        {
            this.broker = broker ?? throw new ArgumentNullException(nameof(broker));
            this.addressConfig = addressConfig ?? throw new ArgumentNullException(nameof(addressConfig));
        }

        public void Invoke(MiddlewareContext context, Action next)
        {
            basicMessage = context.BasicMessage;
            next();
        }

        public void OnError(Exception ex)
        {
            if (basicMessage == null) throw new Exception("Message is null!", ex);
            basicMessage.BodyStream.Position = 0;

            var level = 0;

            while (ex != null)
            {
                var headerName = $"EzBus.ErrorMessage L{level}";
                var value = $"{DateTime.UtcNow}: {ex.Message}";
                basicMessage.AddHeader(headerName, value);
                ex = ex.InnerException;
                level++;
            }

            broker.Send(addressConfig.ErrorAddress, basicMessage);
        }
    }
}