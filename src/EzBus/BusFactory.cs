namespace EzBus
{
    public class BusFactory
    {
        public IBus Run()
        {
            return null;
        }

        public void WithTransport<T>()
            where T : IReceivingChannel
        {

        }
    }
}