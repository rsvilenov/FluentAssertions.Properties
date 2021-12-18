namespace FluentAssertions.Properties.Invocation
{
    internal interface IPropertyInvoker
    {
        void SetValue(string propertyName, object testData);
        object GetValue(string propertyName);
    }

    internal interface IPropertyInvoker<TProperty>
    {
        void SetValue(string propertyName, TProperty testData);
        TProperty GetValue(string propertyName);
    }
}
