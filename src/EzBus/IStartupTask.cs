namespace EzBus
{
    public interface IStartupTask
    {
        void Run(IHostConfig config);
    }
}
