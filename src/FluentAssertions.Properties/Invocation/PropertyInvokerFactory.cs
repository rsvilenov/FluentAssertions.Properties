namespace FluentAssertions.Properties.Invocation
{
    internal class PropertyInvokerFactory : IPropertyInvokerFactory
    {
        public IPropertyInvoker CreatePropertyInvoker<TDeclaringType>(TDeclaringType instance)
        {
            return new ReflectionPropertyInvoker<TDeclaringType>(instance);
        }

        public IPropertyInvoker<TProperty> CreatePropertyInvoker<TDeclaringType, TProperty>(TDeclaringType instance)
        {
            return new ReflectionPropertyInvoker<TDeclaringType, TProperty>(instance);
        }
    }
}
