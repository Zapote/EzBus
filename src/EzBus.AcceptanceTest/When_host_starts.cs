using EzBus.AcceptanceTest.Specifications;
using EzBus.AcceptanceTest.TestHelpers;
using EzBus.Core;
using EzBus.Core.ObjectFactory;
using EzBus.ObjectFactory;
using Xunit;

namespace EzBus.AcceptanceTest
{
    public class When_host_starts
    {
        private readonly IObjectFactory objectFactory = new DefaultObjectFactory();

        public When_host_starts()
        {
            StartupTaskOne.HasStarted = false;
            StartupTaskTwo.HasStarted = false;

            objectFactory.Initialize();
            new Host(new TaskRunner(objectFactory)).Start();
        }

        [Then]
        public void All_startup_tasks_shall_have_run()
        {
            Assert.True(StartupTaskOne.HasStarted);
            Assert.True(StartupTaskTwo.HasStarted);
        }
    }
}