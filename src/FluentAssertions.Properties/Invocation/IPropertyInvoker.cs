namespace FluentAssertions.Properties.Invocation
{
    internal interface IPropertyInvoker<TProperty>
    {
        IInvocationResult SetValue(string propertyName, TProperty value);
        IInvocationResult<TProperty> GetValue(string propertyName);
    }
}
