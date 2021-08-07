using FluentAssertions.Properties.Data;
using System;
using System.Linq;
using System.Reflection;

namespace FluentAssertions.Properties.Selectors
{
    public class InstancePropertyOfValueTypeSelector<TDeclaringType> : InstancePropertySelectorBase<TDeclaringType, InstancePropertyInfo<TDeclaringType>>
    {
        public InstancePropertyOfValueTypeSelector(InstancePropertySelectorBase<TDeclaringType, InstancePropertyInfo<TDeclaringType>> instancePropertySelector)
            : base(instancePropertySelector)
        {
        }

        public InstancePropertyOfValueTypeSelector<TDeclaringType> ThatHaveDefaultValue
        {
            get
            {
                SelectedProperties = SelectedProperties
                    .Where(property => CheckIfValueIsDefault(property.PropertyInfo));

                return this;
            }
        }

        public InstancePropertyOfValueTypeSelector<TDeclaringType> ThatAreNotNullable
        {
            get
            {
                SelectedProperties = SelectedProperties
                    .Where(property => CheckIfValueIsDefault(property.PropertyInfo));

                return this;
            }
        }

        public InstancePropertyOfValueTypeSelector<TDeclaringType> ThatAreNullable
        {
            get
            {
                SelectedProperties = SelectedProperties
                    .Where(property => !CheckIfValueIsDefault(property.PropertyInfo));

                return this;
            }
        }

        private bool CheckIfValueIsDefault(PropertyInfo propertyInfo)
        {
            Type actualType = propertyInfo.PropertyType.CheckIfTypeIsNullableValueType()
                ? propertyInfo.PropertyType.GetActualTypeIfNullable()
                : propertyInfo.PropertyType;

            object defaultValue = Activator
                .CreateInstance(actualType);

            return propertyInfo
                .GetGetMethod()
                .Invoke(Instance, null)
                .Equals(defaultValue);
        }

        ///// <summary>
        ///// Only select the properties that return the specified type
        ///// </summary>
        //public PropertyInvocationCollection<TDeclaringType, object> WhenCalledWithDefaultValue()
        //{
        //    return new PropertyInvocationCollection<TDeclaringType, object>(Instance, default, SelectedProperties);
        //}
    }
}
