namespace EzBus
{
    public interface IStartupTask
    {
        string Name { get; }
        void Run();
    }
}
