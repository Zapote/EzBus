namespace EzBus.AcceptanceTest.Specifications
{
    public abstract class SpecificationBase
    {
        protected SpecificationBase()
        {
            Setup();
        }

        public void Setup()
        {
            Given();
            When();
        }

        protected abstract void When();
        protected virtual void Given() { }
    }
}