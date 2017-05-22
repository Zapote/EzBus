using EzBus.AcceptanceTest.TestHelpers;
using EzBus.Core;
using EzBus.Core.ObjectFactory;
using EzBus.Logging;

namespace EzBus.AcceptanceTest.Specifications
{
    public abstract class BusSpecificationBase
    {
        protected Host host;
        protected FakeMessageChannel messageChannel = new FakeMessageChannel();
        protected FakeMessageRouting messageRouting = new FakeMessageRouting();
        protected CoreBus bus;

        protected BusSpecificationBase()
        {
            bus = new CoreBus(messageChannel, messageChannel, messageRouting);

            var objectFactory = new DefaultObjectFactory();
            objectFactory.Initialize();

            host = objectFactory.GetInstance<Host>();
            host.Start();

            LogManager.SetLogLevel(LogLevel.Verbose);
        }
    }
}