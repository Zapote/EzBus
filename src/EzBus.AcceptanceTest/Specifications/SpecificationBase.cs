namespace EzBus.AcceptanceTest.Specifications
{
    [Specification]
    public abstract class SpecificationBase
    {
        [Given]
        public void Setup()
        {
            Given();
            When();
        }

        protected abstract void When();
        protected virtual void Given() { }
    }
}