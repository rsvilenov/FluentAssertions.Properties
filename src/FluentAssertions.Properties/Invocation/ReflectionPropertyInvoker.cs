namespace FluentAssertions.Properties.Invocation
{
    internal class ReflectionPropertyInvoker<TDeclaringType> : IPropertyInvoker
    {
        private readonly TDeclaringType _instance;
        public ReflectionPropertyInvoker(TDeclaringType instance)
        {
            _instance = instance;
        }

        public void SetValue(string propertyName, object testData)
        {
            SetValueInternal(propertyName, testData);
        }

        public void SetValue<TProperty>(string propertyName, TProperty testData)
        {
            SetValueInternal(propertyName, testData);
        }

        public object GetValue(string propertyName)
        {
            return GetValueInternal(propertyName);
        }
        
        public TProperty GetValue<TProperty>(string propertyName)
        {
           return (TProperty)GetValueInternal(propertyName);
        }

        private void SetValueInternal(string propertyName, object testData)
        {
            _instance.GetType()
                .GetProperty(propertyName)
                .SetValue(_instance, testData);
        }

        public object GetValueInternal(string propertyName)
        {
            return _instance.GetType()
                 .GetProperty(propertyName)
                 .GetValue(_instance);
        }
    }
}
