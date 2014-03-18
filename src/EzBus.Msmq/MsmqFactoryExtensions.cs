namespace EzBus.Msmq
{
    public static class MsmqFactoryExtensions
    {
        public static IBusStarter WithMsmq(this IBusFactory factory)
        {
            factory.Config.SetReceivingChannel(new MsmqReceivingChannel());
            factory.Config.SetSendingChannel(new MsmqSendingChannel());
            return factory as IBusStarter;
        }
    }
}