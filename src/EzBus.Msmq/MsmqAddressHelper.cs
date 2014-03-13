using System;

namespace EzBus.Msmq
{
    public class MsmqAddressHelper
    {
        private const string directPrefix = @"FormatName:DIRECT=OS:";
        private const string tcpPrefix = @"FormatName:DIRECT=TCP:";

        public static string GetQueueName(EndpointAddress address)
        {
            var machineName = address.MachineName;

            if (string.IsNullOrEmpty(address.MachineName))
            {
                machineName = Environment.MachineName;
            }

            return string.Format(@"{0}\private$\{1}", machineName, address.QueueName);
        }

        public static string GetQueuePath(EndpointAddress address)
        {
            return string.Format("{0}{1}", directPrefix, GetQueueName(address));
        }
    }
}