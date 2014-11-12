using System.Runtime.Remoting.Messaging;
using EzBus.Core.Resolvers;
using EzBus.Core.Serilizers;
using EzBus.Logging;
using EzBus.Serilizers;
using System;
using System.Linq;
using System.Reflection;

namespace EzBus.Core
{
    public class Host
    {
        private readonly IMessageChannelResolver messageChannelResolver;
        private static readonly ILogger log = HostLogManager.GetLogger(typeof(Host));
        private readonly IObjectFactory objectFactory;
        private ISubscriptionStorage subscriptionStorage;
        private readonly ISendingChannel sendingChannel;
        private readonly IMessageSerilizer messageSerializer = new XmlMessageSerializer();
        private readonly HandlerCache handlerCache = new HandlerCache();
        private readonly string endpointName;
        private readonly string endpointErrorName;
        private readonly int workerThreads;
        private readonly int numberOfRetrys;

        public Host(HostConfig hostConfig, IMessageChannelResolver messageChannelResolver)
        {
            if (hostConfig == null) throw new ArgumentNullException("hostConfig");
            if (messageChannelResolver == null) throw new ArgumentNullException("messageChannelResolver");
            this.messageChannelResolver = messageChannelResolver;

            objectFactory = hostConfig.ObjectFactory;
            workerThreads = hostConfig.WorkerThreads;
            numberOfRetrys = hostConfig.NumberOfRetrys;

            sendingChannel = messageChannelResolver.GetSendingChannel();

            endpointName = CreateEndpointName();
            endpointErrorName = string.Format("{0}.error", endpointName);
        }

        public void Start()
        {
            log.Debug("Starting Ezbus Host");

            handlerCache.Prime();
            if (handlerCache.NumberOfEntries == 0) return;

            subscriptionStorage = (ISubscriptionStorage)objectFactory.CreateInstance(typeof(ISubscriptionStorage));
            subscriptionStorage.Initialize(new EndpointAddress(endpointName));
            objectFactory.Initialize();

            for (var i = 0; i < workerThreads; i++)
            {
                var receivingChannel = messageChannelResolver.GetReceivingChannel();
                receivingChannel.Initialize(new EndpointAddress(endpointName), new EndpointAddress(endpointErrorName));
                receivingChannel.OnMessageReceived += OnMessageReceived;
            }

            SendSubscriptionMessages();

            log.Debug("EzBus host started");
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

                log.DebugFormat("Invoking handler: {0}", handlerType.Name);

                var result = InvokeHandler(handlerType, message);

                if (!result.Success)
                {
                    PlaceMessageOnErrorQueue(e.Message, result.Exception.InnerException);
                }
            }
        }

        private InvokationResult InvokeHandler(Type handlerType, object message)
        {
            var success = true;
            Exception exception = null;

            for (var i = 0; i < numberOfRetrys; i++)
            {
                var methodInfo = handlerType.GetMethod("Handle", new[] { message.GetType() });
                var messageFilters = new IMessageFilter[0];

                objectFactory.BeginScope();

                try
                {
                    var handler = objectFactory.CreateInstance(handlerType);
                    messageFilters = MessageFilterResolver.GetMessageFilters(objectFactory);

                    messageFilters.Apply(x => x.Before());
                    methodInfo.Invoke(handler, new[] { message });
                    messageFilters.Apply(x => x.After());

                    break;
                }
                catch (Exception ex)
                {
                    log.Error(string.Format("Attempt {1}: Failed to handle message '{0}'.", message.GetType().Name, i + 1), ex.InnerException);
                    success = false;
                    messageFilters.Apply(x => x.OnError(ex.InnerException));
                    exception = ex.InnerException;
                }

                objectFactory.EndScope();
            }

            return new InvokationResult(success, exception);
        }

        private void SendSubscriptionMessages()
        {
            var subscriptions = Config.SubscriptionSection.Section.Subscriptions;

            foreach (Config.SubscriptionElement subscription in subscriptions)
            {
                var subscriptionMessage = new SubscriptionMessage
                {
                    Endpoint = string.Format("{0}@{1}", endpointName, Environment.MachineName)
                };

                var destination = EndpointAddress.Parse(subscription.Endpoint);

                log.DebugFormat("Subscribing to: {0}", destination);
                sendingChannel.Send(destination, ChannelMessageFactory.CreateChannelMessage(subscriptionMessage, messageSerializer));
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

            log.Debug(endpointErrorName);

            sendingChannel.Send(new EndpointAddress(endpointErrorName), message);
        }

        private string CreateEndpointName()
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly == null) return GetType().Assembly.GetName().Name;
            return entryAssembly.GetName().Name;
        }
    }
}
