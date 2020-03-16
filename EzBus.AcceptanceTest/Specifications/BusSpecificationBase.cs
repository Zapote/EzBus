using EzBus.AcceptanceTest.TestHelpers;
using EzBus.Core;
using EzBus.Logging;

namespace EzBus.AcceptanceTest.Specifications
{
    public abstract class BusSpecificationBase
    {
        protected static FakeMessageChannel messageChannel = new FakeMessageChannel();
        protected FakeMessageRouting messageRouting = new FakeMessageRouting();
        protected IBus bus;

        protected BusSpecificationBase()
        {
            //bus = new Bus(messageChannel, messageChannel);

            //var objectFactory = new DefaultObjectFactory();
            //objectFactory.Initialize();

            //host = objectFactory.GetInstance<IHost>();
            //host.Start();

            LogManager.SetLogLevel(LogLevel.Verbose);
        }
    }
}