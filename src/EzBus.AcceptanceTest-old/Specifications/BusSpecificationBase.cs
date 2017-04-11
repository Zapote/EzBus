using EzBus.AcceptanceTest.TestHelpers;
using EzBus.Core;
using EzBus.Core.ObjectFactory;
using EzBus.Logging;

namespace EzBus.AcceptanceTest.Specifications
{
    public abstract class BusSpecificationBase : SpecificationBase
    {
        protected Host host;
        protected CoreBus bus;
        protected FakeMessageChannel messageChannel;
        protected readonly FakeMessageRouting messageRouting = new FakeMessageRouting();

        protected override void Given()
        {
            messageChannel = new FakeMessageChannel();
            bus = new CoreBus(messageChannel, messageChannel, messageRouting);

            var objectFactory = new DefaultObjectFactory();
            objectFactory.Initialize();

            host = objectFactory.GetInstance<Host>();
            host.Start();

            LogManager.SetLogLevel(LogLevel.Off);
        }
    }
}