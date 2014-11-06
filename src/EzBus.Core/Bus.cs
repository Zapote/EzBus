using EzBus;
using EzBus.Core;

// ReSharper disable once CheckNamespace
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

    public static void Send(string destination, object message)
    {
        bus.Send(destination, message);
    }

    public static void Publish(object message)
    {
        bus.Publish(message);
    }
}

