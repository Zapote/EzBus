using System;

namespace EzBus
{
    public interface IAssemblyScanner
    {
        Type[] FindTypes(Type t);
        Type[] FindTypes<T>();
    }
}