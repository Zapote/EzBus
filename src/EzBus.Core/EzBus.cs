using EzBus;
using EzBus.Core;


public class Bus
{
    private static readonly IBus bus;

    static Bus()
    {
        bus = new BusFactory().Start();
    }

    public static void Start() { }

    public static void Send(object message)
    {
        bus.Send(message);
    }

    public static void Publish(object message)
    {
        bus.Publish(message);
    }
}
