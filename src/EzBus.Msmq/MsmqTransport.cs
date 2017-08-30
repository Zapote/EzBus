namespace EzBus.Msmq
{
    public class MsmqTransport : ITransport
    {
        public MsmqTransport(IHost host)
        {
            Host = host;
        }

        public IHost Host { get; }
    }
}