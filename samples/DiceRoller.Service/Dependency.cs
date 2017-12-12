using System;

namespace DiceRoller.Service
{
    public class Dependency : IDependency
    {
        public string Id { get; } = Guid.NewGuid().ToString();
    }
}