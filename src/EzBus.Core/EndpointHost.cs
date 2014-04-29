using System;
using System.Linq;
using System.Reflection;
using EzBus.Core.Serilizers;
using EzBus.Serilizers;

namespace EzBus.Core
{
    public class EndpointHost
    {
        private readonly EndpointConfig endpointConfig;
        private readonly IReceivingChannel receivingChannel;
        private readonly ISendingChannel sendingChannel;
        private readonly IObjectFactory objectFactory;
        private readonly IMessageSerilizer messageSerializer;
        private HandlerCache handlerCache;
        private int numberOfRetrys = 5;
        private string inputQueue;
        private string errorQueue;

        public EndpointHost(EndpointConfig endpointConfig)
        {
            if (endpointConfig == null) throw new ArgumentNullException("endpointConfig");
            this.endpointConfig = endpointConfig;
            receivingChannel = endpointConfig.ReceivingChannel;
            sendingChannel = endpointConfig.SendingChannel;
            objectFactory = endpointConfig.ObjectFactory;
            messageSerializer = new XmlMessageSerializer();
        }

        public void SetNumberOfRetrys(int value)
        {
            numberOfRetrys = value;
        }

        public void Start()
        {
            var scanner = new AssemblyScanner();
            var handlerTypes = scanner.FindTypeInAssemblies(typeof(IHandle<>));
            handlerCache = new HandlerCache();

            foreach (var handlerType in handlerTypes)
            {
                handlerCache.Add(handlerType);
            }

            inputQueue = CreateEndpointName();
            errorQueue = string.Format("{0}.error", inputQueue);
            receivingChannel.Initialize(new EndpointAddress(inputQueue), new EndpointAddress(errorQueue));
            receivingChannel.OnMessageReceived += OnMessageReceived;
        }

        private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            var messageTypeName = e.Message.Headers.ElementAt(0).Value;
            var handlerInfo = handlerCache.GetHandlerInfo(messageTypeName);

            object message = null;

            foreach (var info in handlerInfo)
            {
                var handlerType = info.HandlerType;
                var messageType = info.MessageType;

                if (message == null)
                {
                    message = messageSerializer.Deserialize(e.Message.BodyStream, messageType);
                }

                var result = InvokeHandler(handlerType, message);

                if (!result.Success)
                {
                    PlaceMessageOnErrorQueue(e.Message, result.Exception.InnerException);
                }
            }
        }

        private void PlaceMessageOnErrorQueue(ChannelMessage message, Exception exception)
        {
            var level = 0;

            while (exception != null)
            {
                var headerName = string.Format("ErrorMessage L{0}", level);
                var value = string.Format("{0}: {1}", DateTime.Now, exception.Message);
                message.AddHeader(headerName, value);
                exception = exception.InnerException;
                level++;
            }

            sendingChannel.Send(new EndpointAddress(errorQueue), message);
        }

        private InvokeResult InvokeHandler(Type handlerType, object message)
        {
            var success = true;
            Exception exception = null;
            var methodInfo = handlerType.GetMethod("Handle", new[] { message.GetType() });
            var handler = objectFactory.CreateInstance(handlerType);

            for (var i = 0; i < numberOfRetrys; i++)
            {
                try
                {
                    methodInfo.Invoke(handler, new[] { message });
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error in attempt {0}: {1}", i + 1, ex.InnerException.Message);
                    exception = ex;
                    success = false;
                }
            }

            return new InvokeResult(success, exception);
        }

        public void Stop()
        {

        }

        private string CreateEndpointName()
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly == null) return GetType().Assembly.GetName().Name;
            return entryAssembly.GetName().Name;
        }
    }
}
