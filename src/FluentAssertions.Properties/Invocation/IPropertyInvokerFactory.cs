namespace FluentAssertions.Properties.Invocation
{
    internal interface IPropertyInvokerFactory
    {
        IPropertyInvoker<TProperty> CreatePropertyInvoker<TDeclaringType, TProperty>(TDeclaringType instance);
    }
}
