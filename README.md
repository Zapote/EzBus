EzBus - Messaging made easy!
===============================

## Getting started

#### Install via NuGet

For <b>Msmq</b> transport:<br/>
nuget Install-package EzBus.Msmq

For <b>RabbitMQ</b> transport:<br/>
nuget Install-package EzBus.RabbitMQ

#### Start EzBus
```C#
When your application starts

//EzBus.Msmq
Bus.Configure().UseMsmq;

//EzBus.RabbitMQ
Bus.Configure().UseRabbitMQ;
```

#### Send your message

```C#
Bus.Send("My.Service", new TextMessage { Text = "Hello EzBus" });
```

#### Publish your message

```C#
Bus.Publish(new TextMessage { Text = "Hello EzBus" });
```

#### Subscribe to published messages

in ezbus.config.json:

```javascript
{
  "ezbus": {
    "subscriptions": [
      {
        "endpoint": "My.Service"
      }
    ]
  }
}
```

add endpoints that you want to receive messages from.

#### Handle your message


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
#### Constructur/Dependency injection in handler

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

#### Middleware

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
  }

  public void OnError(Exception ex)
  {
    unitOfWork.Rollback();
  }
}
```
