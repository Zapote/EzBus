using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace EzBus.Core.Test.Tasks
{
  public class TaskRunnerTest
  {
    private TaskRecorder recorder = new TaskRecorder();

    [Fact]
    public void All_system_startup_tasks_shall_run_in_correct_order()
    {
      GetTaskRunner().Run<ISystemStartupTask>();

      Assert.Equal("StartupTaskTwo", recorder.Records()[0]);
      Assert.Equal("StartupTaskOne", recorder.Records()[1]);
    }

    private ITaskRunner GetTaskRunner()
    {
      var services = new ServiceCollection();
      services.AddSingleton(recorder);
      services.AddLogging();
      services.AddScoped<ITaskRunner, TaskRunner>();
      services.AddScoped<IStartupTask, StartupTaskOne>();
      services.AddScoped<IStartupTask, StartupTaskTwo>();
      services.AddScoped<ISystemStartupTask, StartupTaskOne>();
      services.AddScoped<ISystemStartupTask, StartupTaskTwo>();
      var sp = services.BuildServiceProvider();
      return sp.GetService<ITaskRunner>();
    }
  }

  class StartupTaskOne : ISystemStartupTask
  {
    private readonly TaskRecorder recorder;

    public StartupTaskOne(TaskRecorder recorder)
    {
      this.recorder = recorder;
    }

    public string Name => "StartupTaskOne";

    public int Prio => 10;

    public Task Run()
    {
      recorder.Record(this);
      return Task.CompletedTask;
    }
  }

  class StartupTaskTwo : ISystemStartupTask
  {
    private readonly TaskRecorder recorder;

    public StartupTaskTwo(TaskRecorder recorder)
    {
      this.recorder = recorder;
    }

    public string Name => "StartupTaskTwo";
    public int Prio => 100;

    public Task Run()
    {
      recorder.Record(this);
      return Task.CompletedTask;
    }
  }

  class TaskRecorder
  {
    private readonly List<string> records = new List<string>();

    public void Record(ITask t)
    {
      records.Add(t.Name);
    }

    public string[] Records()
    {
      return records.ToArray();
    }
  }
}
