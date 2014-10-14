using System;

namespace EzBus.Msmq
{
    public static class EndpointAddressExtensions
    {
        private const string directPrefix = @"FormatName:DIRECT=OS:";

        public static string GetQueueName(this EndpointAddress address)
        {
            if (address == null) throw new ArgumentNullException("address");

            var machineName = address.MachineName;

            if (string.IsNullOrEmpty(address.MachineName))
            {
                machineName = Environment.MachineName;
            }

            return string.Format(@"{0}\private$\{1}", machineName, address.QueueName);
        }

        public static string GetQueuePath(this EndpointAddress address)
        {
            return string.Format("{0}{1}", directPrefix, GetQueueName(address));
        }
    }
}

