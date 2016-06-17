using EzBus.Core.ObjectFactory;
using EzBus.Core.Test.TestHelpers;
using EzBus.ObjectFactory;
using NUnit.Framework;

namespace EzBus.Core.Test.Specifications
{
    [Specification]
    public class When_then_host_starts : SpecificationBase
    {
        private Host host;
        private readonly IObjectFactory objectFactory = new DefaultObjectFactory();

        protected override void Given()
        {
            StartupTaskOne.HasStarted = false;
            StartupTaskTwo.HasStarted = false;

            objectFactory.Initialize();
            host = new Host(new TaskRunner(objectFactory));
        }

        protected override void When()
        {
            host.Start();
        }

        [Then]
        public void All_startup_tasks_shall_have_run()
        {
            Assert.That(StartupTaskOne.HasStarted, Is.True);
            Assert.That(StartupTaskTwo.HasStarted, Is.True);
        }
    }
}