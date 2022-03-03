using FluentAssertions.Properties.Data;
using FluentAssertions.Properties.Extensions;
using FluentAssertions.Properties.Invocation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace FluentAssertions.Properties.Selectors
{
    [DebuggerNonUserCode]
    public class InstancePropertyOfValueTypeSelector<TDeclaringType> 
        : InstancePropertySelectorBase<TDeclaringType, InstancePropertyInfo<TDeclaringType>, InstancePropertyOfValueTypeSelector<TDeclaringType>>
    {
        internal InstancePropertyOfValueTypeSelector(TDeclaringType instance, IEnumerable<InstancePropertyInfo<TDeclaringType>> instancePropertyInfos)
            : base(instance, instancePropertyInfos)
        {
        }

        public InstancePropertyOfValueTypeSelector<TDeclaringType> ThatHaveDefaultValue
        {
            get
            {
                var filteredProperties = SelectedProperties
                    .Where(property => CheckIfValueIsDefault(property.PropertyInfo));

                return CloneFiltered(filteredProperties);
            }
        }

        public InstancePropertyOfValueTypeSelector<TDeclaringType> ThatAreNotNullable
        {
            get
            {
                var filteredProperties = SelectedProperties
                    .Where(property => !property.PropertyInfo.PropertyType.IsNullableValueType());

                return CloneFiltered(filteredProperties);
            }
        }

        public InstancePropertyOfValueTypeSelector<TDeclaringType> ThatAreNullable
        {
            get
            {
                var filteredProperties = SelectedProperties
                    .Where(property => property.PropertyInfo.PropertyType.IsNullableValueType());

                return CloneFiltered(filteredProperties);
            }
        }

        protected override InstancePropertyOfValueTypeSelector<TDeclaringType> CloneFiltered(IEnumerable<InstancePropertyInfo<TDeclaringType>> filteredProperties)
        {
            return new InstancePropertyOfValueTypeSelector<TDeclaringType>(Instance, filteredProperties);
        }

        private bool CheckIfValueIsDefault(PropertyInfo propertyInfo)
        {
            bool isNullableValueType = propertyInfo.PropertyType.IsNullableValueType();
            Type actualType = isNullableValueType
                ? propertyInfo.PropertyType.GetActualTypeIfNullable()
                : propertyInfo.PropertyType;

            object defaultValue = Activator
                .CreateInstance(actualType);
            IPropertyInvoker propertyInvoker = InvocationContext.PropertyInvokerFactory.CreatePropertyInvoker<TDeclaringType>(Instance);
            object value = propertyInvoker.GetValue(propertyInfo.Name);
            
            return isNullableValueType 
                ? value == null
                : value.Equals(defaultValue);
        }
    }
}
