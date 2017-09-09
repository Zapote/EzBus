using EzBus.AcceptanceTest.TestHelpers;
using EzBus.Core;
using EzBus.Core.ObjectFactory;
using EzBus.Logging;

namespace EzBus.AcceptanceTest.Specifications
{
    public abstract class BusSpecificationBase
    {
        protected IHost host;
        protected static FakeMessageChannel messageChannel = new FakeMessageChannel();
        protected FakeMessageRouting messageRouting = new FakeMessageRouting();
        protected CoreBus bus;

        protected BusSpecificationBase()
        {
            bus = new CoreBus(messageChannel, messageChannel);

            var objectFactory = new DefaultObjectFactory();
            objectFactory.Initialize();

            host = objectFactory.GetInstance<IHost>();
            host.Start();

            LogManager.SetLogLevel(LogLevel.Verbose);
        }
    }
}