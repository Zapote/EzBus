namespace EzBus
{
    public interface IHandle<in T> : IMessageHandler
    {
        void Handle(T m);
    }
}