EzBus - Messaging made easy!
===============================

### Install via NuGet

##### Msmq
nuget install-package EzBus.Msmq

##### RabbitMQ
nuget install-package EzBus.RabbitMQ

### Start EzBus
When your application starts

##### RabbitMQ 

```C#
using EzBus.RabbitMQ;

Bus.Configure().UseRabbitMQ();
```

##### Msmq: 
```C#
using EzBus.Msmq;

Bus.Configure().UseMsmq();
```

### Subscribe to published messages

```C#
Bus.Subscribe("My.Service");
```

### Send your message

```C#
Bus.Send("My.Service", new TextMessage { Text = "Hello EzBus" });
```

### Publish your message

```C#
Bus.Publish(new TextMessage { Text = "Hello EzBus" });
```

### Handle your message


##### Handler class

```C#
public class TextMessageHandler : IHandle<TextMessage>
{
  public void Handle(TextMessage message)
  {
    Console.WriteLine(message.Text);
  }
}
```
### Constructur/Dependency injection in handler

##### Handler code

```C#
public class TextMessageHandler : IHandle<TextMessage>
{
  private IDependencyService dependencyService;
  
  public TextMessageHandler(IDependencyService dependencyService)
  {
    this.dependencyService = dependencyService;
  }
  ...
}
```

##### ServiceRegistry
Derive from class ServiceRegistry and register dependencies in constructor. This class will be activated on startup.
```C#
public class CoreRegistry : ServiceRegistry
{
    public CoreRegistry()
    {
        // Per message scoped
        Register<IDependencyService, DependencyService>();
        
        // Always unique instance
        Register<IFoo, Foo>().As.Unique();
        
        // Singleton
        Register<IBar, Bar>().As.Singleton();
    }
}
```

### Middleware

The "Invoke" method is called. Place your call before calling next() to do your stuff before mesasage is handled and after if the code should be executed after the message is handled. If an error occurs the "OnError" method is called with the given exception.

Can be used for a UnitOfWork for example. 

```C#
public class UnitOfWorkMiddleware : IMiddleware
{
  private IUnitOfWork unitOfWork;

  public UnitOfWorkMessageFilter(IUnitOfWork unitOfWork)
  {
    this.unitOfWork = unitOfWork;   
  }

  public void Invoke()
  {
    unitOfWork.Start();
    next();
    unitOfWork.Commit();
  }

  public void OnError(Exception ex)
  {
    unitOfWork.Rollback();
  }
}
```

### StartupTasks

If you want EzBus to run a something at startup, implement interface EzBus.IStartupTask

```C#

public class MyStartupTask
{
  public string Name => "MyStartupTask";

  public void Run()
  {
    //Do some exciting stuff
  }
}

```

### Unit Testing

First in setup, start the host.

```C#
Bus.Configure().Host.Start();
```

Create a class that implements interface EzBus.IBus, this class will now be used instead of EzBus implementation when doing bus operations (send, publish).

```C#
public class FakeBus : IBus
{
    public void Send(string destinationQueue, object message)
    {
        
    }

    public void Publish(object message)
    {
        
    }
}
```