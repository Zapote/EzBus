using System;

namespace EzBus
{
    public interface IAssemblyScanner
    {
        Type[] GetMessageHandlers();
    }
}