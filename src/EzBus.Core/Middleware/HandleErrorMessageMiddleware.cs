using System;

namespace EzBus.Core.Middleware
{
    internal class HandleErrorMessageMiddleware : ISystemMiddleware
    {
        private readonly ISendingChannel sendingChannel;
        private readonly IBusConfig busConfig;
        private ChannelMessage channelMessage;

        public HandleErrorMessageMiddleware(ISendingChannel sendingChannel, IBusConfig busConfig)
        {
            if (sendingChannel == null) throw new ArgumentNullException(nameof(sendingChannel));
            if (busConfig == null) throw new ArgumentNullException(nameof(busConfig));
            this.sendingChannel = sendingChannel;
            this.busConfig = busConfig;
        }

        public void Invoke(MiddlewareContext context, Action next)
        {
            channelMessage = context.ChannelMessage;
            next();
        }

        public void OnError(Exception ex)
        {
            var level = 0;

            while (ex != null)
            {
                var headerName = $"EzBus.ErrorMessage L{level}";
                var value = $"{DateTime.UtcNow}: {ex.Message}";
                channelMessage.AddHeader(headerName, value);
                ex = ex.InnerException;
                level++;
            }

            var endpointAddress = new EndpointAddress(busConfig.ErrorEndpointName);
            sendingChannel.Send(endpointAddress, channelMessage);
        }
    }
}