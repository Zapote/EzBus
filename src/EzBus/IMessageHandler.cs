namespace EzBus
{
    public interface IMessageHandler<in T>
    {
        void Handle(T message);
    }
}