EzBus - Messaging made easy!
===============================

## Getting started

#### Install


##### NuGet

For MSMQ transport:<br/>
nuget Install-package EzBus.Msmq

#### Send your message

```C#
Bus.Send("ez.service.queue", new TextMessage { Text = "Hello EzBus" });
```

#### Publish your message

```C#
Bus.Publish(new TextMessage { Text = "Hello EzBus" });
```

#### Subscribe to messages

in app.config:

```xml
  <configSections>
    <section name="subscriptions" type="EzBus.Core.Config.SubscriptionSection, EzBus.Core"/>
  </configSections>

  <subscriptions>
    <add endpoint="EzBus.Samples.Service"/>
  </subscriptions>
```

add endpoints that you want tot receive messages from.

#### Handle your message

```C#
public class TextMessageHandler : IHandle<TextMessage>
{
  public void Handle(TextMessage message)
  {
    Console.WriteLine(message.Text);
  }
}
```

