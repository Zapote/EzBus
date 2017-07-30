using System;

namespace EzBus.AcceptanceTest.TestHelpers
{
    public class ToBeReceivedMessage
    {
        public Guid Id { get; } = Guid.NewGuid();
    }
}
