using System;

namespace EzBus.Utils
{
    public interface IAssemblyScanner
    {
        Type[] FindTypes(Type t);
        Type[] FindTypes<T>();
    }
}