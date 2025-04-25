namespace EzBus.Core.Routing
{
    public interface IMessageRouting
    {
        string GetRoute(string @namespace, string messageType);
    }
}