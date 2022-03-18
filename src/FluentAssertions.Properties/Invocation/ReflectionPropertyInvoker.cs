using System;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace FluentAssertions.Properties.Invocation
{
    internal class ReflectionPropertyInvoker<TDeclaringType, TProperty> : IPropertyInvoker<TProperty>
    {
        private readonly TDeclaringType _instance;
        public ReflectionPropertyInvoker(TDeclaringType instance)
        {
            _instance = instance;
        }

        public IInvocationResult SetValue(string propertyName, TProperty value)
        {
            try
            {
                PropertyInfo propInfo = GetPropertyInfo(propertyName);
                propInfo.SetValue(_instance, value);
                return new InvocationResult();
            }
            catch (TargetInvocationException tex)
            {
                return new InvocationResult(ExceptionDispatchInfo.Capture(tex.InnerException));
            }
        }

        public IInvocationResult<TProperty> GetValue(string propertyName)
        {
            try
            {
                object valueObj = GetPropertyInfo(propertyName).GetValue(_instance);
                TProperty value = valueObj != default ? (TProperty)valueObj : default;
                return new PropertyInvocationResult<TProperty>(value);
            }
            catch (TargetInvocationException tex)
            {
                return new PropertyInvocationResult<TProperty>(ExceptionDispatchInfo.Capture(tex.InnerException));
            }
        }

        private PropertyInfo GetPropertyInfo(string propertyName)
        {
            var propInfo = _instance
                            .GetType()
                            .GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (propInfo == null)
            {
                throw new ArgumentException($"Property {propertyName} cannot be found in type {typeof(TDeclaringType)}");
            }

            return propInfo;
        }
    }
}
