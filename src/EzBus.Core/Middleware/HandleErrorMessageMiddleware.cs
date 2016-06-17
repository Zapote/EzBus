using System;

namespace EzBus.Core.Middleware
{
    internal class HandleErrorMessageMiddleware : ISystemMiddleware
    {
        private readonly ISendingChannel sendingChannel;
        private readonly IHostConfig hostConfig;
        private ChannelMessage channelMessage;

        public HandleErrorMessageMiddleware(ISendingChannel sendingChannel, IHostConfig hostConfig)
        {
            if (sendingChannel == null) throw new ArgumentNullException(nameof(sendingChannel));
            if (hostConfig == null) throw new ArgumentNullException(nameof(hostConfig));
            this.sendingChannel = sendingChannel;
            this.hostConfig = hostConfig;
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

            var endpointAddress = new EndpointAddress(hostConfig.ErrorEndpointName);
            sendingChannel.Send(endpointAddress, channelMessage);
        }
    }
}