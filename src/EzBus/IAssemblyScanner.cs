using System;

namespace EzBus
{
    public interface IAssemblyScanner
    {
        Type[] FindType(Type t);
        Type[] FindType<T>();
    }
}