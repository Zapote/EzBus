using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EzBus.Utils;
using Microsoft.Extensions.Logging;

namespace EzBus.Core.Utils
{
  public class AssemblyScanner : IAssemblyScanner
  {
    private readonly List<Assembly> assemblies = new List<Assembly>();
    private readonly IAssemblyFinder assemblyFinder;
    private readonly ILogger<AssemblyScanner> logger;

    public AssemblyScanner(IAssemblyFinder assemblyFinder, ILogger<AssemblyScanner> logger)
    {
      this.assemblyFinder = assemblyFinder ?? throw new ArgumentNullException(nameof(assemblyFinder));
      this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

      LoadAssemblies();
    }

    public Type[] FindTypes<T>()
    {
      return FindTypes(typeof(T));
    }

    public Type[] FindTypes(Type t)
    {
      var types = new List<Type>();
      for (var i = 0; i < assemblies.Count; i++)
      {
        var assembly = assemblies[i];

        try
        {
          var definedTypes = assembly.DefinedTypes;

          foreach (var typeInfo in definedTypes)
          {
            var type = typeInfo.AsType();
            if (t == type) continue;

            if (t.IsInterface())
            {
              var interfaces = typeInfo.GetInterfaces();
              if (interfaces.Length == 0) continue;

              var any = interfaces.Any(x => x.Name == t.Name && x.Namespace == t.Namespace);

              if (!any) continue;
            }

            if (!t.IsAssignableFrom(type) && !t.IsInterface()) continue;

            types.Add(type);
          }
        }
        catch (Exception ex)
        {
          logger.LogWarning($"Failed to scan assemby: {assembly.FullName}", ex);
          assemblies.Remove(assembly);
        }
      }

      return types.OrderBy(x => x.FullName).ToArray();
    }

    private void LoadAssemblies()
    {
      assemblies.Clear();
      assemblies.AddRange(assemblyFinder.FindAssemblies());
    }
  }
}
