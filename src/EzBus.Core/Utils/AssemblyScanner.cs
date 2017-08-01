using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using EzBus.Logging;
using EzBus.Utils;
using Microsoft.Extensions.DependencyModel;

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

            foreach (var assembly in assemblies)
            {
                try
                {
                    var definedTypes = assembly.DefinedTypes;

                    foreach (var typeInfo in definedTypes)
                    {
                        var type = typeInfo.AsType();
                        if (t == type) continue;

                        if (t.IsInterface())
                        {
                            var handlerInterface = typeInfo.GetInterface(t.Name);
                            if (handlerInterface == null) continue;
                        }

                        if (!t.IsAssignableFrom(type) && !t.IsInterface()) continue;

                        types.Add(type);
                    }
                }
                catch (Exception ex)
                {
                    log.Error($"Failed to scan assemby: {assembly.FullName}", ex);
                }
            }

            return types.OrderBy(x => x.FullName).ToArray();
        }

        private static void LoadAssemblies()
        {
            if (assemblyLoaded) return;
            assemblyLoaded = true;

            var libs = DependencyContext.Default.RuntimeLibraries;
            var ezlibs = libs.Where(x => x.Name.StartsWith("EzBus"));

            foreach (var lib in ezlibs)
            {
                var assembly = Assembly.Load(new AssemblyName(lib.Name));
                assemblies.Add(assembly);
                log.Verbose($"Referenced assembly added: {assembly.FullName}");
            }

            var directory = AppContext.BaseDirectory;
            var fileNames = new List<string>();
            fileNames.AddRange(Directory.GetFiles(directory, "*.dll", SearchOption.TopDirectoryOnly));
            fileNames.AddRange(Directory.GetFiles(directory, "*.exe", SearchOption.TopDirectoryOnly));

            foreach (var fileName in fileNames)
            {
                var fileInfo = new FileInfo(fileName);
                var assembly = Assembly.Load(new AssemblyName(fileInfo.Name.Replace(fileInfo.Extension, "")));
                if (assemblies.Any(x => x.FullName == assembly.FullName)) continue;
                assemblies.Add(assembly);
                log.Verbose($"Scanned assembly added: {assembly.FullName}");
            }
        }
    }
}
