namespace EzBus.Core
{
    public interface ISystemStartupTask : IStartupTask
    {
        int Prio { get; }
    }
}