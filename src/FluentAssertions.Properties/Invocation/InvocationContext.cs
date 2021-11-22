namespace FluentAssertions.Properties.Invocation
{
    internal static class InvocationContext
    {
        internal static IPropertyInvokerFactory PropertyInvokerFactory { get; set; } 
            =  new PropertyInvokerFactory();
    }
}
