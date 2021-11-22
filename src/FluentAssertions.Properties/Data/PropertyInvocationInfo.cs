using System;
using System.Reflection;

namespace FluentAssertions.Properties.Data
{
    public class PropertyInvocationInfo<TDeclaringType, TProperty>
    {
        internal PropertyInvocationInfo(PropertyInfo propertyInfo,
            Func<TProperty> valueDelegate)
        {
            PropertyInfo = propertyInfo;
            ValueDelegate = valueDelegate;
        }

        public PropertyInfo PropertyInfo { get; }

        public Func<TProperty> ValueDelegate { get; }
    }
}
