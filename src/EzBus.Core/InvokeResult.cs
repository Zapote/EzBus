using System;

namespace EzBus.Core
{
    public class InvokeResult
    {
        public InvokeResult(bool success, Exception exception)
        {
            Success = success;
            Exception = exception;
        }

        public bool Success { get; set; }
        public Exception Exception { get; set; }
    }
}