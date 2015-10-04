namespace EzBus.Core.Resolvers
{
    public class ObjectFactoryResolver : ResolverBase<IObjectFactory>
    {
        private static readonly ObjectFactoryResolver resolver = new ObjectFactoryResolver();
        private static IObjectFactory factoryInstance;

        public static IObjectFactory GetObjectFactory()
        {
            return factoryInstance ?? (factoryInstance = resolver.GetInstance());
        }
    }
}