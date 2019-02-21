namespace EzBus
{
    public interface ISubscriptionManager
    {
        void Subscribe(string endpoint);
        void Subscribe(string endpoint, string messageName);
        void Unsubscribe(string endpoint);
        void Unsubscribe(string endpoint, string messageName);
    }
}
