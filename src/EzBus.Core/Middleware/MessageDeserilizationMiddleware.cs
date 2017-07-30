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
            this.messageSerializer = messageSerializer ?? throw new ArgumentNullException(nameof(messageSerializer));
            this.handlerCache = handlerCache ?? throw new ArgumentNullException(nameof(handlerCache));
        }

        public void Invoke(MiddlewareContext context, Action next)
        {
            var channelMessage = context.ChannelMessage;
            var messageTypeName = channelMessage.GetHeader(MessageHeaders.MessageFullname);
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