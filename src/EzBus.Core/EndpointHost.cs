using System;
using System.Linq;
using System.Reflection;
using EzBus.Core.Serilizers;
using EzBus.Serilizers;

namespace EzBus.Core
{
    public class EndpointHost
    {
        private readonly EndpointConfig config;
        private readonly IMessageSerilizer messageSerializer;
        private HandlerCache handlerCache;

        public EndpointHost(EndpointConfig config)
        {
            if (config == null) throw new ArgumentNullException("config");
            this.config = config;
            messageSerializer = new XmlMessageSerializer();
        }

        public void Start()
        {
            var scanner = new AssemblyScanner();
            var handlerTypes = scanner.FindTypeInAssemblies(typeof(IMessageHandler<>));
            handlerCache = new HandlerCache();

            foreach (var handlerType in handlerTypes)
            {
                handlerCache.Add(handlerType);
            }

            var receiver = config.ReceivingChannel;
            receiver.Initialize(new EndpointAddress(CreateEndpointName()));
            receiver.OnMessageReceived += OnMessageReceived;
        }

        private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            var handlerInfo = handlerCache.GetHandlerInfo(e.Message.Headers.ElementAt(0).Value);

            object message = null;

            foreach (var info in handlerInfo)
            {
                var handlerType = info.HandlerType;
                var messageType = info.MessageType;

                if (message == null)
                {
                    message = messageSerializer.Deserialize(e.Message.BodyStream, messageType);
                }

                var methodInfo = handlerType.GetMethod("Handle", new[] { messageType });
                var handler = Activator.CreateInstance(handlerType);

                methodInfo.Invoke(handler, new[] { message });
            }
        }

        public void Stop()
        {

        }

        private static string CreateEndpointName()
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            return entryAssembly.GetName().Name;
        }
    }

}
