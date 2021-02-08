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

var bus = BusFactory
          .Address("my-service")
          .UseRabbitMQ()
          .LogLevel(LogLevel.Debug)
          .CreateBus();

await bus.Start();
```

### Subscribe to published messages

```C#
await bus.Subscribe("order-service", "OrderPlaced");
```

### Send your message

```C#
await bus.Send("my-service", new TextMessage { Text = "Hello EzBus" });
```

### Publish your message

```C#
await bus.Publish(new TextMessage { Text = "Hello EzBus" });
```

### Handle your message


##### Handler class

```C#
public class TextMessageHandler : IHandle<TextMessage>
{
  public Task Handle(TextMessage message)
  {
    Console.WriteLine(message.Text);
    return Task.CompletedTask;
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

  public async Task Invoke()
  {
    await unitOfWork.StartAsync();
    await next();
    await unitOfWork.CommitAsync();
  }

  public Task OnError(Exception ex)
  {
    await unitOfWork.RollbackAsync();
  }
}
```

### StartupTasks

If you want EzBus to run a something at startup, implement interface EzBus.IStartupTask

```C#

public class MyTask : IStartupTask
{
    public string Name => "My task";

    public Task Run()
    {
        return Task.CompletedTask;
    }
}

```

### Unit Testing

Create a class that implements interface EzBus.IBus, this class will now be used instead of EzBus implementation when doing bus operations (send, publish).

```C#
public class FakeBus : IBus
{
    public Task Send(string destinationQueue, object message)
    {
        return Task.CompletedTask;
    }

    public Task Publish(object message)
    {
        return Task.CompletedTask;
    }
}
```