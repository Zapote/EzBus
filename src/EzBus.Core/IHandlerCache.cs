using System;
using System.Collections.Generic;

namespace EzBus.Core
{
    public interface IHandlerCache
    {
        void Add(Type handlerType);
        IEnumerable<HandlerInfo> GetHandlerInfo(string messageTypeName);
        void Prime();
        bool HasCustomHandlers();
        int NumberOfEntries { get; }
    }
}