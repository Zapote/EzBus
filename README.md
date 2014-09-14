EzBus - Messaging made easy!
===============================

## Getting started: 
-------------------------------

#### Send your message

```C#
Bus.Send("messageservice", new StartTheBus());
```

#### Publish your message

```C#
Bus.Send("messageservice", new BusStarted("EasyBus"));
```

#### Handle your message

```C#
public class BusStartedHandler : IHandle<BusStarted> 
{
  public void Handle(BusStarted message)
  {
    Console.WriteLine("Bus {0} started", message.BusName);
  }
}
```

