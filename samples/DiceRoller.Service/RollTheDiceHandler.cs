using System;
using EzBus;

namespace DiceRoller.Service
{
    public class RollTheDiceHandler : IHandle<RollTheDice>
    {
        public void Handle(RollTheDice message)
        {
            Console.WriteLine(message.Number);
        }
    }
}