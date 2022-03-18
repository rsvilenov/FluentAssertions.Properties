using FluentAssertions.Execution;
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
    /// <summary>
    /// Allows for fluent selection of value type properties.
    /// <typeparamref name="TDeclaringType">The type of the instance.</typeparamref>
    /// </summary>
    [DebuggerNonUserCode]
    public class InstancePropertyOfValueTypeSelector<TDeclaringType> 
        : InstancePropertySelectorBase<TDeclaringType, InstancePropertyInfo<TDeclaringType>, InstancePropertyOfValueTypeSelector<TDeclaringType>>
    {
        internal InstancePropertyOfValueTypeSelector(TDeclaringType instance, IEnumerable<InstancePropertyInfo<TDeclaringType>> instancePropertyInfos)
            : base(instance, instancePropertyInfos)
        {
        }

        /// <summary>
        /// Only select the properties whose current value is the default for their type.
        /// </summary>
        public InstancePropertyOfValueTypeSelector<TDeclaringType> ThatHaveDefaultValue
        {
            get
            {
                List<InstancePropertyInfo<TDeclaringType>> filteredProperties = new();

                using (AssertionScope scope = new())
                {
                    foreach (var instancePropInfo in SelectedProperties)
                    {
                        bool success = TryCheckIfValueIsDefault(instancePropInfo.PropertyInfo, out bool isDefault, out Exception ex);
                        if (!success)
                        {
                            Execute.Assertion
                                .FailWith($"Did not expect any exceptions for property {instancePropInfo.PropertyInfo.Name}, but got {ex}.");
                        }
                        else if (isDefault)
                        {
                            filteredProperties.Add(instancePropInfo);
                        }
                        
                    }
                }

                return CloneFiltered(filteredProperties);
            }
        }

        /// <summary>
        /// Only select the properties that are not of nullable value types.
        /// </summary>
        public InstancePropertyOfValueTypeSelector<TDeclaringType> ThatAreNotNullable
        {
            get
            {
                var filteredProperties = SelectedProperties
                    .Where(property => !property.PropertyInfo.PropertyType.IsNullableValueType());

                return CloneFiltered(filteredProperties);
            }
        }

        /// <summary>
        /// Only select the properties that are of nullable value types.
        /// </summary>
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

        private bool TryCheckIfValueIsDefault(PropertyInfo propertyInfo, out bool isDefault, out Exception ex)
        {
            bool isNullableValueType = propertyInfo.PropertyType.IsNullableValueType();
            Type actualType = isNullableValueType
                ? propertyInfo.PropertyType.GetActualTypeIfNullable()
                : propertyInfo.PropertyType;

            object defaultValue = Activator
                .CreateInstance(actualType);
            IPropertyInvoker<object> propertyInvoker = InvocationContext.PropertyInvokerFactory.CreatePropertyInvoker<TDeclaringType, object>(Instance);
            IInvocationResult<object> getResult = propertyInvoker.GetValue(propertyInfo.Name);
            if (!getResult.Success)
            {
                ex = getResult.ExceptionDispatchInfo.SourceException;
                isDefault = false;
                return false;
            }
            else
            {
                ex = null;
                isDefault = isNullableValueType
                    ? getResult.Value == null
                    : getResult.Value.Equals(defaultValue);
                return true;
            }
        }
    }
}
