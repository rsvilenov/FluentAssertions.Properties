namespace FluentAssertions.Properties.Invocation
{
    internal class PropertyInvokerFactory
    {
        internal IPropertyInvoker CreatePropertyInvoker<TDeclaringType>(TDeclaringType instance)
        {
            return new ReflectionPropertyInvoker<TDeclaringType>(instance);
        }
    }
}
