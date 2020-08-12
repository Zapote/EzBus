using System;
using System.Threading.Tasks;

namespace DiceRoller.Worker
{
    public interface IMyClass
    {
        Guid Id { get; set; }
        Task DoSomeWork();
    }
}
