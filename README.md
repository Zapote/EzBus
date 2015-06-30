EzBus - Messaging made easy!
===============================

## Getting started

#### Install via NuGet

For <b>Msmq</b> transport:<br/>
nuget Install-package EzBus.Msmq

For <b>Azure ServiceBus</b> transport:<br/>
nuget Install-package EzBus.WindowsAzure.ServiceBus

#### Send your message

```C#
Bus.Send("My.Service", new TextMessage { Text = "Hello EzBus" });
```

Configure destinations in app.config:

```xml
  <configSections>
    <section name="destinations" type="EzBus.Core.Config.DestinationSection, EzBus.Core"/>
  </configSections>

   <destinations>
    <add assembly="Business.Messages" endpoint="My.Service"/>
  </destinations>
```
```C#
Bus.Send(new TextMessage { Text = "Hello EzBus" });
```

#### Publish your message

```C#
Bus.Publish(new TextMessage { Text = "Hello EzBus" });
```

#### Subscribe to published messages

in app.config:

```xml
  <configSections>
    <section name="subscriptions" type="EzBus.Core.Config.SubscriptionSection, EzBus.Core"/>
  </configSections>

  <subscriptions>
    <add endpoint="My.Service"/>
  </subscriptions>
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

#### Message Filter

The "Before" method is called before the message is handled and the "After" method is called right after the message is handled. If an error occurs the "OnError" method is called with the given exception.

Can be used for a UnitOfWork for example. 

```C#
public class UnitOfWorkMessageFilter : IMessageFilter
{
  private IUnitOfWork unitOfWork;

  public UnitOfWorkMessageFilter(IUnitOfWork unitOfWork)
  {
    this.unitOfWork = unitOfWork;   
  }

  public void Before()
  {
    unitOfWork.Start();
  }

  public void After()
  {
    unitOfWork.Commit();
  }

  public void OnError(Exception ex)
  {
    unitOfWork.Rollback();
  }
}
```
