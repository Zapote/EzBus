using System;
using System.Linq;
using EzBus.Core.Builders;

namespace EzBus.Core
{
    public class MessageChannelResolver
    {
        private static Type sendingChannelType;
        private static Type receivingChannelType;

        static MessageChannelResolver()
        {
            ResolveTypes();
        }

        public static ISendingChannel GetSendingChannel()
        {
            return (ISendingChannel)new DefaultObjectFactory().CreateInstance(sendingChannelType);
        }

        public static IReceivingChannel GetReceivingChannel()
        {
            return (IReceivingChannel)new DefaultObjectFactory().CreateInstance(receivingChannelType);
        }

        private static void ResolveTypes()
        {
            var assemblyScanner = new AssemblyScanner();
            var sendingChannels = assemblyScanner.FindType<ISendingChannel>();
            var localns = typeof(MessageChannelResolver).Namespace;

            foreach (var type in sendingChannels.Where(type => type.Namespace != localns))
            {
                sendingChannelType = type;
                break;
            }

            if (sendingChannelType == null)
            {
                //channelTypes.SendingChannelType = new 
            }

            var receivingChannels = assemblyScanner.FindType<IReceivingChannel>();

            foreach (var type in receivingChannels.Where(type => type.Namespace != localns))
            {
                receivingChannelType = type;
                break;
            }

            if (sendingChannelType == null)
            {
                //channelTypes.SendingChannelType = new 
            }
        }
    }
}