using System;

namespace EzBus
{
    public class EndpointAddress
    {
        public string QueueName { get; private set; }
        public string MachineName { get; private set; }

        public EndpointAddress(string queueName, string machineName = "")
        {
            if (string.IsNullOrEmpty(queueName)) throw new ArgumentNullException("queueName");
            QueueName = queueName.ToLower();
            MachineName = machineName;
        }

        private bool Equals(EndpointAddress other)
        {
            return string.Equals(QueueName, other.QueueName) && string.Equals(MachineName, other.MachineName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((EndpointAddress)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((QueueName != null ? QueueName.GetHashCode() : 0) * 397) ^ (MachineName != null ? MachineName.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(MachineName) ? QueueName : string.Format("{0}@{1}", QueueName, MachineName);
        }

        public static EndpointAddress Parse(string s)
        {
            var parts = s.Split('@');
            return parts.Length > 1 ? new EndpointAddress(parts[0], parts[1]) : new EndpointAddress(parts[0]);
        }

    }
}