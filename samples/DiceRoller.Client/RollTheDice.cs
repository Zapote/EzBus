using System;

namespace DiceRoller.Client
{
    public class RollTheDice
    {
        public RollTheDice()
        {
            Number = new Random((int)DateTime.Now.Ticks).Next(1934838, 192923753);
        }

        public int Number { get; private set; }
    }
}