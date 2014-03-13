namespace EzBus
{
    public interface IBus
    {
        void Send(object message);
        void Publish(object message);
    }
}
