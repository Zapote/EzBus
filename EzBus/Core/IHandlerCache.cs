using System;

namespace EzBus.Core
{
    public interface IHandlerCache
    {
        void Add(Type handlerType);
        HandlerInfo[] GetHandlerInfo(string messageFullName);
        void Prime();
        int NumberOfEntries { get; }
    }
}