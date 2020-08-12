using System.Collections.Generic;
using System.Reflection;

namespace EzBus.Utils
{
  public interface IAssemblyFinder
  {
    IEnumerable<Assembly> FindAssemblies();
  }
}