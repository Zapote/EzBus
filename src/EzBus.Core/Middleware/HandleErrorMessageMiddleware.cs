using System;

namespace EzBus.Core.Middleware
{
    internal class HandleErrorMessageMiddleware : IPreMiddleware
    {
        private readonly ISendingChannel sendingChannel;
        private readonly IBusConfig busConfig;
        private ChannelMessage channelMessage;

        public HandleErrorMessageMiddleware(ISendingChannel sendingChannel, IBusConfig busConfig)
        {
            this.sendingChannel = sendingChannel ?? throw new ArgumentNullException(nameof(sendingChannel));
            this.busConfig = busConfig ?? throw new ArgumentNullException(nameof(busConfig));
        }

        public void Invoke(MiddlewareContext context, Action next)
        {
            channelMessage = context.ChannelMessage;
            next();
        }

        public void OnError(Exception ex)
        {
            if (channelMessage == null) throw new Exception("ChannelMessage is null!", ex);

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