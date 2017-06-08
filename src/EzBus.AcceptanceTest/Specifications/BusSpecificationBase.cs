using EzBus.AcceptanceTest.TestHelpers;
using EzBus.Core;
using EzBus.Core.ObjectFactory;
using EzBus.Logging;

namespace EzBus.AcceptanceTest.Specifications
{
    public abstract class BusSpecificationBase
    {
        protected BusStarter busStarter;
        protected FakeMessageChannel messageChannel = new FakeMessageChannel();
        protected FakeMessageRouting messageRouting = new FakeMessageRouting();
        protected CoreBus bus;

        protected BusSpecificationBase()
        {
            bus = new CoreBus(messageChannel, messageChannel, messageRouting);

            var objectFactory = new DefaultObjectFactory();
            objectFactory.Initialize();

            busStarter = objectFactory.GetInstance<BusStarter>();
            busStarter.Start();

            LogManager.SetLogLevel(LogLevel.Verbose);
        }
    }
}