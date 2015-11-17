namespace EzBus.Core.Test.TestHelpers
{
    public class StartupTaskOne : IStartupTask
    {
        public static bool HasStarted { get; set; }

        public void Run()
        {
            HasStarted = true;
        }
    }
}
