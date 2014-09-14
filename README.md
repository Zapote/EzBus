EzBus - Messaging made easy!
===============================

## Getting started: 
-------------------------------

#### Send your message

```C#
Bus.Send("messageservice", new StartTheBus("EasyBus"));
```

#### Publish your message

```C#
Bus.Publish(new BusStarted("EasyBus"));
```

#### Handle your message

```C#
public class StartTheBusHandler : IHandle<StartTheBus>
{
  public void Handle(StartTheBus message)
  {
    Bus.Publish(new BusStarted("EasyBus));
  }
}
```

