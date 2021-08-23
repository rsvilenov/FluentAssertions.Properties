namespace FluentAssertions.Properties.Invocation
{
    internal interface IPropertyInvoker
    {
        void SetValue<TProperty>(string propertyName, TProperty testData);
        TProperty GetValue<TProperty>(string propertyName);
    }
}
