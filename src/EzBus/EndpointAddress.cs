namespace EzBus
{
    public class EndpointAddress
    {
        public string QueueName { get; private set; }
        public string MachineName { get; private set; }

        public EndpointAddress(string queueName, string machineName)
        {
            QueueName = queueName.ToLower();
            MachineName = machineName;
        }

        public EndpointAddress(string queueName)
            : this(queueName, string.Empty)
        {

        }

        public override string ToString()
        {
            return string.Format("{0}@{1}", QueueName, MachineName);
        }
    }
}