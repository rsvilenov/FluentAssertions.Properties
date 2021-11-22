namespace FluentAssertions.Properties.Invocation
{
    internal class PropertyInvokerFactory : IPropertyInvokerFactory
    {
        public IPropertyInvoker CreatePropertyInvoker<TDeclaringType>(TDeclaringType instance)
        {
            return new ReflectionPropertyInvoker<TDeclaringType>(instance);
        }
    }
}
