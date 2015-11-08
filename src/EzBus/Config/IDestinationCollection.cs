namespace EzBus.Config
{
    public interface IDestinationCollection
    {
        IDestination this[int index] { get; set; }
    }
}