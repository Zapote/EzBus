using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace EzBus.Core
{
    public class AssemblyScanner : IAssemblyScanner
    {
        public Type[] FindTypeInAssemblies(Type typeToFind)
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            var directory = Path.GetDirectoryName(executingAssembly.Location) ?? "\\.";
            var assemblyFiles = new List<string>();
            assemblyFiles.AddRange(Directory.GetFiles(directory, "*.dll", SearchOption.TopDirectoryOnly));
            assemblyFiles.AddRange(Directory.GetFiles(directory, "*.exe", SearchOption.TopDirectoryOnly));
            var types = new List<Type>();

            foreach (var file in assemblyFiles)
            {
                var assembly = Assembly.LoadFile(file);

                foreach (var type in assembly.GetTypes())
                {
                    var handlerInterface = type.GetInterface(typeToFind.Name);
                    if (handlerInterface == null) continue;
                    types.Add(type);
                }
            }

            return types.ToArray();
        }
    }
}
