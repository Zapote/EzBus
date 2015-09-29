using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web;
using EzBus.Logging;

namespace EzBus.Core.Utils
{
    public class AssemblyScanner : IAssemblyScanner
    {
        private static readonly ILogger log = HostLogManager.GetLogger(typeof(AssemblyScanner));
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
                    var assembly = Assembly.LoadFile(file);
                    foreach (var type in assembly.GetTypes())
                    {
                        if (t == type) continue;

                        if (t.IsInterface)
                        {
                            var handlerInterface = type.GetInterface(t.Name);
                            if (handlerInterface == null) continue;
                        }

                        if (!t.IsAssignableFrom(type) && !t.IsInterface) continue;

                        types.Add(type);
                    }
                }
                catch (Exception ex)
                {
                    log.Error(string.Format("Failed to scan assemby: {0}", file), ex);
                }

            }

            return types.ToArray();
        }

        private static void LoadAssemblyFiles()
        {

            if (directoryScanned) return;
            string directory;
            var httpContext = HttpContext.Current;

            if (httpContext != null)
            {
                directory = httpContext.Server.MapPath("/bin");
            }
            else
            {
                var executingAssembly = Assembly.GetExecutingAssembly();
                directory = Path.GetDirectoryName(executingAssembly.Location) ?? "\\.";
            }

            assemblyFiles.AddRange(Directory.GetFiles(directory, "*.dll", SearchOption.TopDirectoryOnly));
            assemblyFiles.AddRange(Directory.GetFiles(directory, "*.exe", SearchOption.TopDirectoryOnly));
            directoryScanned = true;
        }
    }
}
