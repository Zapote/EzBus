namespace EzBus
{
    public interface ITransport
    {
        IHost Host { get; }
    }

    public class NullTransport : ITransport
    {
        public NullTransport(IHost host)
        {
            Host = host;
        }

        public IHost Host { get; }
    }
}
