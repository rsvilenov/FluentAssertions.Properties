namespace FluentAssertions.Properties.Invocation
{
    internal class PropertyInvokerFactory : IPropertyInvokerFactory
    {
        public IPropertyInvoker<TProperty> CreatePropertyInvoker<TDeclaringType, TProperty>(TDeclaringType instance)
        {
            return new ReflectionPropertyInvoker<TDeclaringType, TProperty>(instance);
        }
    }
}
