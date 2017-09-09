namespace EzBus.Msmq
{
    public static class TransportExtensions
    {
        public static void UseMsmq(this ITransport transport)
        {
            transport.Host.Start();
        }
    }
}
