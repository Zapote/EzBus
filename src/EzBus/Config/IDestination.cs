namespace EzBus.Config
{
    public interface IDestination
    {
        string Assembly { get; set; }
        string Endpoint { get; set; }
        string Message { get; set; }
    }
}