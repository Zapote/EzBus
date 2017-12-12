using EzBus.ObjectFactory;

namespace DiceRoller.Service
{
    public class Registry : ServiceRegistry
    {
        public Registry()
        {
            Register<IDependency, Dependency>();
        }
    }
}