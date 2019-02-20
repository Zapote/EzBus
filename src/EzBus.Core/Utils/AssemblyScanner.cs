using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EzBus.Logging;
using EzBus.Utils;

namespace EzBus.Core.Utils
{
    public class AssemblyScanner : IAssemblyScanner
    {
        private static readonly ILogger log = LogManager.GetLogger(typeof(AssemblyScanner));
        private static readonly List<Assembly> assemblies = new List<Assembly>();
        private static bool assemblyLoaded;

        public AssemblyScanner()
        {
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

                            var any = interfaces.Any(x => x.Name == t.Name);

                            if (!any) continue;
                        }

                        if (!t.IsAssignableFrom(type) && !t.IsInterface()) continue;

                        types.Add(type);
                    }
                }
                catch (Exception ex)
                {
                    log.Warn($"Failed to scan assemby: {assembly.FullName}", ex);
                    assemblies.Remove(assembly);
                }
            }

            return types.OrderBy(x => x.FullName).ToArray();
        }

        private static void LoadAssemblies()
        {
            if (assemblyLoaded) return;
            assemblyLoaded = true;

            var assemblyFinder = new AssemblyFinder();
            assemblies.AddRange(assemblyFinder.FindAssemblies());
        }
    }
}
