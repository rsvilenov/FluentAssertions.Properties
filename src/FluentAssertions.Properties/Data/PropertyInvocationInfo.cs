using FluentAssertions.Properties.Invocation;
using System;
using System.Reflection;

namespace FluentAssertions.Properties.Data
{
    public class PropertyInvocationInfo<TDeclaringType, TProperty>
    {
        internal PropertyInvocationInfo(PropertyInfo propertyInfo,
            Func<IInvocationResult<TProperty>> valueSourceInvocationDelegate)
        {
            PropertyInfo = propertyInfo;
            ValueSourceInvocationDelegate = valueSourceInvocationDelegate;
        }

        public PropertyInfo PropertyInfo { get; }

        public Func<IInvocationResult<TProperty>> ValueSourceInvocationDelegate { get; }
    }
}
