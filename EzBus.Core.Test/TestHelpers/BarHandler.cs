using System.Threading.Tasks;

namespace EzBus.Core.Test.TestHelpers
{
    public class BarHandler : IHandle<TestMessage>
    {
        public Task Handle(TestMessage message)
        {
            return Task.CompletedTask;
        }
    }
}