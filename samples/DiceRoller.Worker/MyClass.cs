using System;

namespace DiceRoller.Worker
{
    public class MyClass : IMyClass
    {
        public MyClass()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
    }
}
