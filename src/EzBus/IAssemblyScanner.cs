using System;

namespace EzBus
{
    public interface IAssemblyScanner
    {
        Type[] FindTypeInAssemblies(Type typeToFind);
    }
}