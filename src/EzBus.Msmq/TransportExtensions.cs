// ReSharper disable once CheckNamespace
namespace EzBus
{
    public static class TransportExtensions
    {
        public static void UseMsmq(this ITransport transport)
        {
            transport.Host.Start();
        }
    }
}
