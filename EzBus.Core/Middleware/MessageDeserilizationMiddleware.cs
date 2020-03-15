using System;
using System.Linq;
using EzBus.Logging;
using EzBus.Serializers;

namespace EzBus.Core.Middleware
{
    internal class MessageDeserilizationMiddleware : IPreMiddleware
    {
        private static readonly ILogger log = LogManager.GetLogger<MessageDeserilizationMiddleware>();
        private readonly IBodySerializer bodySerializer;
        private readonly IHandlerCache handlerCache;

        public MessageDeserilizationMiddleware(IBodySerializer bodySerializer, IHandlerCache handlerCache)
        {
            this.bodySerializer = bodySerializer ?? throw new ArgumentNullException(nameof(bodySerializer));
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
                context.Message = bodySerializer.Deserialize(channelMessage.BodyStream, messageType);
            }
            else
            {
                context.Message = bodySerializer.Deserialize(channelMessage.BodyStream, typeof(object));
            }

            next();
        }

        public void OnError(Exception ex)
        {
        }
    }
}