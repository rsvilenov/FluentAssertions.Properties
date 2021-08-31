namespace FluentAssertions.Properties.Invocation
{
    internal interface IPropertyInvoker
    {
        void SetValue<TProperty>(string propertyName, TProperty testData);
        void SetValue(string propertyName, object testData);
        TProperty GetValue<TProperty>(string propertyName);
        object GetValue(string propertyName);
    }
}
