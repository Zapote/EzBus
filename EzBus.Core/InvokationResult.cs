using System;

namespace EzBus.Core
{
    public class InvokationResult(bool success, Exception ex)
    {
        public bool Success { get; set; } = success;
        public Exception Exception { get; set; } = ex;
    }
}