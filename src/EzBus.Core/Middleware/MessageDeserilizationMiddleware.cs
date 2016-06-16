using System;
using System.Linq;
using EzBus.Logging;
using EzBus.Serializers;

namespace EzBus.Core.Middleware
{
    internal class MessageDeserilizationMiddleware : IPreMiddleware
    {
        private static readonly ILogger log = LogManager.GetLogger<MessageDeserilizationMiddleware>();
        private readonly IMessageSerializer messageSerializer;
        private readonly IHandlerCache handlerCache;

        public MessageDeserilizationMiddleware(IMessageSerializer messageSerializer, IHandlerCache handlerCache)
        {
            if (messageSerializer == null) throw new ArgumentNullException(nameof(messageSerializer));
            if (handlerCache == null) throw new ArgumentNullException(nameof(handlerCache));
            this.messageSerializer = messageSerializer;
            this.handlerCache = handlerCache;
        }

        public void Invoke(MiddlewareContext context, Action next)
        {
            var channelMessage = context.ChannelMessage;
            var messageTypeName = channelMessage.GetHeader(MessageHeaders.MessageType);
            var handlerInfos = handlerCache.GetHandlerInfo(messageTypeName).ToList();

            if (handlerInfos.Any())
            {
                var messageType = handlerInfos.First().MessageType;
                context.Message = messageSerializer.Deserialize(channelMessage.BodyStream, messageType);
            }
            else
            {
                context.Message = messageSerializer.Deserialize(channelMessage.BodyStream, typeof(object));
            }

            next();
        }

        public void OnError(Exception ex)
        {
        }
    }
}