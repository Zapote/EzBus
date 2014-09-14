using System;

namespace EzBus
{
    public interface IMessageFilter
    {
        void Before();
        void After(Exception ex);
    }
}