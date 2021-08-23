namespace FluentAssertions.Properties.Invocation
{
    internal class ReflectionPropertyInvoker<TDeclaringType> : IPropertyInvoker
    {
        private readonly TDeclaringType _instance;
        public ReflectionPropertyInvoker(TDeclaringType instance)
        {
            _instance = instance;
        }

        public void SetValue<TProperty>(string propertyName, TProperty testData)
        {
            _instance.GetType()
                .GetProperty(propertyName)
                .GetSetMethod()
                .Invoke(_instance, new object[] { testData });
        }

        public TProperty GetValue<TProperty>(string propertyName)
        {
           return (TProperty)_instance.GetType()
                .GetProperty(propertyName)
                .GetGetMethod()
                .Invoke(_instance, null);
        }
    }
}
