using System.Threading.Tasks;

namespace EzBus.Core.Test.TestHelpers
{
    public class FooHandler : IHandle<TestMessageData>
    {
        public Task Handle(TestMessageData message)
        {
            return Task.CompletedTask;
        }
    }
}