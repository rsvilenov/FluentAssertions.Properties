using System.Reflection;

namespace FluentAssertions.Properties.Data
{
    public class InstancePropertyInfo<TDeclaringType>
    {
        public InstancePropertyInfo() { }
        public InstancePropertyInfo(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
        }

        internal InstancePropertyInfo(InstancePropertyInfo<TDeclaringType> instancePropertyInfo)
            : this(instancePropertyInfo.PropertyInfo)
            { }

        public PropertyInfo PropertyInfo { get; internal set; }
    }

    public class InstancePropertyInfo<TDeclaringType, TProperty> : InstancePropertyInfo<TDeclaringType>
    {
        public InstancePropertyInfo() { }
        public InstancePropertyInfo(InstancePropertyInfo<TDeclaringType> instancePropertyInfo)
            : base(instancePropertyInfo)
        {
        }

        internal TProperty PropertyValue { get; set; }
    }
}
