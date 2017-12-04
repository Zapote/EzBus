using System;
using EzBus;
using EzBus.ObjectFactory;

namespace DiceRoller.Service
{
    public class RollTheDiceHandler : IHandle<RollTheDice>
    {
        private readonly IDependency dependency;

        public RollTheDiceHandler(IDependency dependency)
        {
            this.dependency = dependency;
        }

        public void Handle(RollTheDice message)
        {
            Console.WriteLine("DependencyId " + dependency.Id);
            Console.WriteLine($"Rolling the dice {message.Attempts} times");
            for (var i = 0; i < message.Attempts; i++)
            {
                var result = new Random().Next(1, 7);
                Bus.Publish(new DiceRolled { Result = result });
            }
        }
    }

    public class MyMiddleware : IMiddleware
    {
        private readonly IDependency dependency;

        public MyMiddleware(IDependency dependency)
        {
            this.dependency = dependency;
        }

        public void Invoke(MiddlewareContext context, Action next)
        {
            Console.WriteLine("Before " + dependency.Id);
            next();
            Console.WriteLine("After " + dependency.Id);
        }

        public void OnError(Exception ex)
        {
            throw new NotImplementedException();
        }
    }

    public class Dependency : IDependency
    {
        public string Id { get; } = Guid.NewGuid().ToString();
    }

    public interface IDependency
    {
        string Id { get; }
    }

    public class Registry : ServiceRegistry
    {
        public Registry()
        {
            Register<IDependency, Dependency>();
        }
    }
}