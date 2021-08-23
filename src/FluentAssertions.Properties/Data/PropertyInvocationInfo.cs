using System.Reflection;

namespace FluentAssertions.Properties.Data
{
    public class PropertyInvocationInfo<TDeclaringType, TProperty>
    {
        internal PropertyInvocationInfo(PropertyInfo propertyInfo,
            TProperty value)
        {
            PropertyInfo = propertyInfo;
            Value = value;
        }

        public PropertyInfo PropertyInfo { get; }

        public TProperty Value { get; }
    }
}
