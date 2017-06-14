using System;
using EzBus;

namespace DiceRoller.Service
{
    public class RollTheDiceHandler : IHandle<RollTheDice>
    {
        public void Handle(RollTheDice message)
        {
            
            Console.WriteLine($"Rolling the dice {message.Attempts} times");
            for (var i = 0; i < message.Attempts; i++)
            {
                var seed = Guid.NewGuid().GetHashCode();
                var result = new Random().Next(1, 7);
                Bus.Publish(new DiceRolled { Result = result });
            }
        }
    }
}