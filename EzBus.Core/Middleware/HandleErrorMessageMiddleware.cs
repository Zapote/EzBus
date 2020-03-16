using System;

namespace EzBus.Core.Middleware
{
    internal class HandleErrorMessageMiddleware : IPreMiddleware
    {
        private readonly ISender sender;
        private readonly IConfig busConfig;
        private BasicMessage basicMessage;

        public HandleErrorMessageMiddleware(ISender sender, IConfig busConfig)
        {
            this.sender = sender ?? throw new ArgumentNullException(nameof(sender));
            this.busConfig = busConfig ?? throw new ArgumentNullException(nameof(busConfig));
        }

        public void Invoke(MiddlewareContext context, Action next)
        {
            basicMessage = context.BasicMessage;
            next();
        }

        public void OnError(Exception ex)
        {
            if (basicMessage == null) throw new Exception("ChannelMessage is null!", ex);
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

            sender.Send(busConfig.ErrorAddress, basicMessage);
        }
    }
}