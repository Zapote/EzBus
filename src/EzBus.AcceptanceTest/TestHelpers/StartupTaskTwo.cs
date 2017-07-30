namespace EzBus.AcceptanceTest.TestHelpers
{
    public class StartupTaskTwo : IStartupTask
    {
        public static bool HasStarted { get; set; }

        public string Name => "StartupTaskTwo";

        public void Run()
        {
            HasStarted = true;
        }
    }
}