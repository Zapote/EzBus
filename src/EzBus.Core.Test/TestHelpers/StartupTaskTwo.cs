namespace EzBus.Core.Test.TestHelpers
{
    public class StartupTaskTwo : IStartupTask
    {
        public static bool HasStarted { get; set; }

        public void Run()
        {
            HasStarted = true;
        }
    }
}