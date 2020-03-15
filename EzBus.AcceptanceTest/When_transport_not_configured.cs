using System;
using EzBus.AcceptanceTest.Specifications;
using Xunit;

namespace EzBus.AcceptanceTest
{
    public class When_transport_not_configured
    {
        private Exception ex = null;

        public When_transport_not_configured()
        {
            try
            {
                Bus.Send("dark.side.of.the.moon", new { Message = "Hello moon!" });
            }
            catch (Exception e)
            {
                ex = e;
            }

        }

        [Then]
        public void Then_the_message_should_end_up_in_correct_destination()
        {
            Assert.NotNull(ex);
            Assert.Equal("Transport not configured! Pls first call Bus.Configure.UseRabbitMQ() or Bus.Configure.Msmq()", ex.Message);
        }
    }
}