﻿using EzBus.Core.Test.TestHelpers;
using EzBus.Core.Utils;
using Xunit;

namespace EzBus.Core.Test.Utils
{
  public class AssemblyScannerTest
  {
    private readonly AssemblyScanner scanner;

    public AssemblyScannerTest()
    {
      scanner = new AssemblyScanner(new AssemblyFinder(new NullLogger<AssemblyFinder>()), new NullLogger<AssemblyScanner>());
    }

    [Fact]
    public void Scanner_should_find_all_handlers_types()
    {
      var handlerTypes = scanner.FindTypes(typeof(IHandle<>));
      Assert.Contains(typeof(BarHandler), handlerTypes);
      Assert.Contains(typeof(FooHandler), handlerTypes);
    }
  }
}
