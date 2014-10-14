using EzBus;
using EzBus.Core;

public class Bus
{
    private static IBus bus;

    static Bus()
    {
        Start();
    }

    public static void Start()
    {
        if (bus != null) return;
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

