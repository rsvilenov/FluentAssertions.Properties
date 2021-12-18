namespace FluentAssertions.Properties.Invocation
{
    internal interface IPropertyInvokerFactory
    {
        IPropertyInvoker CreatePropertyInvoker<TDeclaringType>(TDeclaringType instance);

        IPropertyInvoker<TProperty> CreatePropertyInvoker<TDeclaringType, TProperty>(TDeclaringType instance);
    }
}
