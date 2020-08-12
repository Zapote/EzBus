using System;
using System.Threading.Tasks;

namespace DiceRoller.Worker
{
    public class MyClass : IMyClass
    {
        public MyClass()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public Task DoSomeWork()
        {
            System.Console.WriteLine("Doing the work");
            return Task.CompletedTask;
        }
    }
}
