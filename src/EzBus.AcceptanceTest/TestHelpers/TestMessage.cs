using System;

namespace EzBus.AcceptanceTest.TestHelpers
{
    public class TestMessage
    {
        public Guid Id { get; } = Guid.NewGuid();
    }
}
