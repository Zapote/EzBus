using System.Threading.Tasks;
using EzBus;

namespace DiceRoller.Worker
{
    public class AnotherHandler : IHandle<RollTheDice>
    {
        private readonly IMyClass myClass;

        public AnotherHandler(IMyClass myClass)
        {
            this.myClass = myClass;
        }

        public Task Handle(RollTheDice message)
        {
            return myClass.DoSomeWork();
        }
    }
}
