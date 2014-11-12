using System;

namespace EzBus
{
    public interface IMessageFilter
    {
        void Before();
        void After();
        void OnError(Exception ex);
    }
}