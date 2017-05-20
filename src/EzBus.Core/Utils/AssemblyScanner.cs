using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using EzBus.Logging;
using EzBus.Utils;

namespace EzBus.Core.Utils
{
    public class AssemblyScanner : IAssemblyScanner
    {
        private static readonly ILogger log = LogManager.GetLogger(typeof(AssemblyScanner));
        private static readonly List<string> assemblyFiles = new List<string>();
        private static bool directoryScanned;

        public AssemblyScanner()
        {
            LoadAssemblyFiles();
        }

        public Type[] FindTypes<T>()
        {
            return FindTypes(typeof(T));
        }

        public Type[] FindTypes(Type t)
        {
            var types = new List<Type>();

            foreach (var file in assemblyFiles)
            {
                try
                {
                    var fileInfo = new FileInfo(file);
                    var assembly = Assembly.Load(new AssemblyName(fileInfo.Name.Replace(fileInfo.Extension, "")));

                    foreach (var type in assembly.GetTypes())
                    {
                        if (t == type) continue;

                        if (t.IsInterface())
                        {
                            var handlerInterface = type.GetInterface(t.Name);
                            if (handlerInterface == null) continue;
                        }

                        if (!t.IsAssignableFrom(type) && !t.IsInterface()) continue;

                        types.Add(type);
                    }
                }
                catch (Exception ex)
                {
                    log.Error($"Failed to scan assemby: {file}", ex);
                }
            }

            return types.OrderBy(x => x.FullName).ToArray();
        }

        private static void LoadAssemblyFiles()
        {
            if (directoryScanned) return;

            var directory = AppContext.BaseDirectory;

            assemblyFiles.AddRange(Directory.GetFiles(directory, "*.dll", SearchOption.TopDirectoryOnly));
            assemblyFiles.AddRange(Directory.GetFiles(directory, "*.exe", SearchOption.TopDirectoryOnly));
            directoryScanned = true;
        }
    }
}
