using System.Reflection;

namespace FluentAssertions.Properties.Invocation
{
    internal class ReflectionPropertyInvoker<TDeclaringType, TProperty> : IPropertyInvoker<TProperty>
    {
        private readonly IPropertyInvoker _nonGenericPropertyInvoker;

        public ReflectionPropertyInvoker(TDeclaringType instance)
        {
            _nonGenericPropertyInvoker = new ReflectionPropertyInvoker<TDeclaringType>(instance);
        }

        public TProperty GetValue(string propertyName)
        {
            return (TProperty)_nonGenericPropertyInvoker.GetValue(propertyName);
        }

        public void SetValue(string propertyName, TProperty testData)
        {
            _nonGenericPropertyInvoker.SetValue(propertyName, testData);
        }
    }


    internal class ReflectionPropertyInvoker<TDeclaringType> : IPropertyInvoker
    {
        private readonly TDeclaringType _instance;
        public ReflectionPropertyInvoker(TDeclaringType instance)
        {
            _instance = instance;
        }

        public void SetValue(string propertyName, object testData)
        {
            try
            {
                _instance.GetType()
                    .GetProperty(propertyName)
                    ?.SetValue(_instance, testData);
            }
            catch (TargetInvocationException tex)
            {
                throw tex.InnerException;
            }
        }

        public object GetValue(string propertyName)
        {
            try
            {
                return _instance.GetType()
                     .GetProperty(propertyName)
                     ?.GetValue(_instance);
            }
            catch (TargetInvocationException tex)
            {
                throw tex.InnerException;
            }
        }
    }
}
