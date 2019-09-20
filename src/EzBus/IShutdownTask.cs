namespace EzBus
{
    public interface IShutdownTask
    {
        string Name { get; }
        void Run();
    }
}