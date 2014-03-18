namespace EzBus.Core.Test.TestHelpers
{
    public class FooHandler : IMessageHandler<MockData>
    {
        public void Handle(MockData message)
        {

        }
    }
}