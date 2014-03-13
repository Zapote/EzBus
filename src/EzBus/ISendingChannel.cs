namespace EzBus
{
    public interface ISendingChannel
    {
        void Send(EndpointAddress destination, MessageEnvelope message);
    }
}