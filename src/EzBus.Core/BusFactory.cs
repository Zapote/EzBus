using EzBus.Core.Resolvers;
using EzBus.Core.Routing;

namespace EzBus.Core
{
    //public class BusFactory : IBusFactory
    //{
    //    public IBus Build()
    //    {
    //        return CreateBus();
    //    }

    //    private static CoreBus CreateBus()
    //    {
    //        var objectFactory = ObjectFactoryResolver.Get();
    //        var sendingChannel = objectFactory.GetInstance<ISendingChannel>();
    //        var publishingChannel = objectFactory.GetInstance<IPublishingChannel>();
    //        return new CoreBus(sendingChannel, publishingChannel, new ConfigurableMessageRouting());
    //    }
    //}
}