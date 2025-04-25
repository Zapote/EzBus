using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using EzBus.Utils;
using Microsoft.Extensions.Logging;

namespace EzBus.Core.Utils
{
  public class AssemblyFinder : IAssemblyFinder
  {
    private readonly List<Assembly> assemblies = new List<Assembly>();
    private readonly ILogger<AssemblyFinder> logger;

    public AssemblyFinder(ILogger<AssemblyFinder> logger)
    {
      this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public IEnumerable<Assembly> FindAssemblies()
    {
      assemblies.Clear();

      FindInAppDomain();
      FindInFiles();

      return assemblies;
    }

    private void FindInAppDomain()
    {
      foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
      {
        AddAssembly(assembly);
      }
    }

    private void FindInFiles()
    {
      FindInFiles(AppDomain.CurrentDomain.BaseDirectory);

      var privateBinPath = AppDomain.CurrentDomain.RelativeSearchPath;
      if (string.IsNullOrEmpty(privateBinPath)) return;

      FindInFiles(privateBinPath);
    }

    private void FindInFiles(string path)
    {
      var fileNames = new List<string>();
      fileNames.AddRange(Directory.GetFiles(path, "*.dll", SearchOption.TopDirectoryOnly));
      fileNames.AddRange(Directory.GetFiles(path, "*.exe", SearchOption.TopDirectoryOnly));

      foreach (var fileName in fileNames)
      {
        var fileInfo = new FileInfo(fileName);
        try
        {
          var assembly = Assembly.Load(new AssemblyName(fileInfo.Name.Replace(fileInfo.Extension, "")));
          AddAssembly(assembly);
        }
        catch (Exception ex)
        {
          logger.LogWarning($"Failed  to load assembly '{fileName}'. {ex.Message}");
        }
      }
    }

    private void AddAssembly(Assembly assembly)
    {
      if (assemblies.Any(x => x.FullName == assembly.FullName)) return;
      assemblies.Add(assembly);
    }
  }
}
