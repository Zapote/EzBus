using EzBus.AcceptanceTest.TestHelpers;
using EzBus.Core;
using EzBus.Core.ObjectFactory;
using EzBus.Logging;

namespace EzBus.AcceptanceTest.Specifications
{
    public abstract class BusSpecificationBase
    {
        protected IBusStarter busStarter;
        protected static FakeMessageChannel messageChannel = new FakeMessageChannel();
        protected FakeMessageRouting messageRouting = new FakeMessageRouting();
        protected CoreBus bus;

        protected BusSpecificationBase()
        {
            bus = new CoreBus(messageChannel, messageChannel, messageRouting);

            var objectFactory = new DefaultObjectFactory();
            objectFactory.Initialize();

            busStarter = objectFactory.GetInstance<IBusStarter>();
            busStarter.Start();

            LogManager.SetLogLevel(LogLevel.Verbose);
        }
    }
}