using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace EzBus.Core.Utils
{
    public class AssemblyFinder
    {
        private readonly List<Assembly> assemblies = new List<Assembly>();

        public IEnumerable<Assembly> FindAssemblies()
        {
            assemblies.Clear();

            FindInExecutingAssembly();
            FindInFiles();

            return assemblies;
        }

        private void FindInExecutingAssembly()
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            var referencedAssemblies = executingAssembly.GetReferencedAssemblies();

            AddAssembly(executingAssembly);

            foreach (var assemblyName in referencedAssemblies)
            {
                AddAssembly(Assembly.Load(assemblyName));
            }
        }

        private void FindInFiles()
        {
            FindInFiles(AppDomain.CurrentDomain.BaseDirectory);
            FindInFiles(AppDomain.CurrentDomain.RelativeSearchPath);
        }

        private void FindInFiles(string path)
        {
            var fileNames = new List<string>();
            fileNames.AddRange(Directory.GetFiles(path, "*.dll", SearchOption.TopDirectoryOnly));
            fileNames.AddRange(Directory.GetFiles(path, "*.exe", SearchOption.TopDirectoryOnly));

            foreach (var fileName in fileNames)
            {
                var fileInfo = new FileInfo(fileName);
                var assembly = Assembly.Load(new AssemblyName(fileInfo.Name.Replace(fileInfo.Extension, "")));
                AddAssembly(assembly);
            }
        }

        private void AddAssembly(Assembly assembly)
        {
            if (assemblies.Any(x => x.FullName == assembly.FullName)) return;
            assemblies.Add(assembly);
        }
    }
}
