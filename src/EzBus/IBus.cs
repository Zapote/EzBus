namespace EzBus
{
    public interface IBus
    {
        void Send(string endpoint, object message);
        void Publish(object message);
    }
}
