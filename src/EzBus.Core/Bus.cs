using System.Threading.Tasks;
using EzBus;
using EzBus.Core;

// ReSharper disable once CheckNamespace
public static class Bus
{
    private static readonly IBus bus;
    private static Host host;

    static Bus()
    {
        if (bus != null) return;
        var factory = new BusFactory();
        bus = factory.Start();
    }

    public static void Start()
    {
        host = new HostFactory().Build();
        host.Start();
    }

    public static void Send(object message)
    {
        bus.Send(message);
    }

    public static async Task SendAsync(object message)
    {
        await Task.Factory.StartNew(() => bus.Send(message));
    }

    public static void Send(string destination, object message)
    {
        bus.Send(destination, message);
    }

    public static async Task SendAsync(string destination, object message)
    {
        await Task.Factory.StartNew(() => bus.Send(destination, message));
    }

    public static void Publish(object message)
    {
        bus.Publish(message);
    }

    public static async Task PublishAsync(object message)
    {
        await Task.Factory.StartNew(() => bus.Publish(message));
    }
}

