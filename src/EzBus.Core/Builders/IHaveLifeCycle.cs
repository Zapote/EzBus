namespace EzBus.Core.Builders
{
    public interface IHaveLifeCycle
    {
        void Singleton();
        void Unique();
    }
}