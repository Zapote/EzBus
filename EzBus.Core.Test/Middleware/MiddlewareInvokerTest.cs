using System.IO;
using System.Threading.Tasks;
using EzBus.Core.Middlewares;
using Xunit;

namespace EzBus.Core.Test.Middleware
{
    public class MiddlewareInvokerTest
    {
        private readonly MiddlewareInvoker invoker = new MiddlewareInvoker(new IMiddleware[] { new MiddlewareOne(), new MiddlewareTwo() });

        [Fact]
        public async Task Test()
        {
            await invoker.Invoke(new MiddlewareContext(new BasicMessage(new MemoryStream())));

            Assert.All(invoker.Middlewares, item =>
            {
                var mv = (TestableMiddleware)item;
                Assert.True(mv.IsInvoked);
            });
        }
    }
}