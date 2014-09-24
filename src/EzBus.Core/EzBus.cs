using EzBus;
using EzBus.Core;


public class Bus
{
    private static IBus bus;

    static Bus()
    {

    }

    public static void Start()
    {
        bus = new BusFactory().Start();
    }

    public static void Send(object message)
    {
        bus.Send(message);
    }

    public static void Publish(object message)
    {
        bus.Publish(message);
    }
}
