namespace EzBus.Core.Routing
{
    public interface IMessageRouting
    {
        string GetRoute(string asssemblyName, string messageType);
    }
}