﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web;

namespace EzBus.Core
{
    public class AssemblyScanner : IAssemblyScanner
    {
        private static List<string> assemblyFiles;
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
                catch (Exception)
                {
                    continue; //TODO: Logging
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
            assemblyFiles = new List<string>();
            assemblyFiles.AddRange(Directory.GetFiles(directory, "*.dll", SearchOption.TopDirectoryOnly));
            assemblyFiles.AddRange(Directory.GetFiles(directory, "*.exe", SearchOption.TopDirectoryOnly));
            directoryScanned = true;
        }
    }
}
