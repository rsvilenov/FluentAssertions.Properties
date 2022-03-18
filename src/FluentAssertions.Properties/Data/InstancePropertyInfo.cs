using System;
using System.Reflection;

namespace FluentAssertions.Properties.Data
{
    public class InstancePropertyInfo<TDeclaringType>
    {
        internal InstancePropertyInfo(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
        }

        internal InstancePropertyInfo(InstancePropertyInfo<TDeclaringType> instancePropertyInfo)
            : this(instancePropertyInfo.PropertyInfo)
            { }

        internal PropertyInfo PropertyInfo { get; set; }

        internal Type InstanceType => typeof(TDeclaringType);
    }

    public class InstancePropertyInfo<TDeclaringType, TProperty> : InstancePropertyInfo<TDeclaringType>
    {
        internal InstancePropertyInfo(InstancePropertyInfo<TDeclaringType> instancePropertyInfo)
            : base(instancePropertyInfo)
        {
        }
    }
}
