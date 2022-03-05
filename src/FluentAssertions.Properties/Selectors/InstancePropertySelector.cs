using FluentAssertions.Properties.Data;
using FluentAssertions.Properties.Extensions;
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
    public class InstancePropertySelector<TDeclaringType> 
        :  InstancePropertySelectorBase<TDeclaringType, InstancePropertyInfo<TDeclaringType>, InstancePropertySelector<TDeclaringType>>
    {
        internal InstancePropertySelector(TDeclaringType instance, IEnumerable<string> propertyNames = null)
        {
            Instance = instance;

            var properties = new List<InstancePropertyInfo<TDeclaringType>>();

            Type type = instance.GetType();
            foreach (PropertyInfo propInfo in type.GetPublicOrInternalProperties())
            {
                if (propertyNames == null || propertyNames.Contains(propInfo.Name))
                {
                    properties.Add(new InstancePropertyInfo<TDeclaringType>(propInfo));
                }
            }


            SelectedProperties = properties;
        }

        private InstancePropertySelector(TDeclaringType instance, IEnumerable<InstancePropertyInfo<TDeclaringType>> instancePropertyInfos)
            : base(instance, instancePropertyInfos)
        {
        }

        /// <summary>
        /// Only select the properties of the specified type.
        /// </summary>
        public InstancePropertyOfKnownTypeSelector<TDeclaringType, TProperty> OfType<TProperty>()
        {
            var selectedProperties = SelectedProperties
                .Where(property => property.PropertyInfo.PropertyType == typeof(TProperty))
                .Select(p => new InstancePropertyInfo<TDeclaringType, TProperty>(p));

            return new InstancePropertyOfKnownTypeSelector<TDeclaringType, TProperty>(Instance, selectedProperties);
        }

        /// <summary>
        /// Only select the properties that are not of the specified type
        /// </summary>
        public InstancePropertySelector<TDeclaringType> NotOfType<TProperty>()
        {
            var filteredProperties = SelectedProperties
                .Where(property => property.PropertyInfo.PropertyType != typeof(TProperty));

            return CloneFiltered(filteredProperties);
        }

        protected override InstancePropertySelector<TDeclaringType> CloneFiltered(IEnumerable<InstancePropertyInfo<TDeclaringType>> filteredProperties)
        {
            return new InstancePropertySelector<TDeclaringType>(Instance, filteredProperties);
        }

        /// <summary>
        /// Only select the properties that are of value type.
        /// </summary>
        public InstancePropertyOfValueTypeSelector<TDeclaringType> ThatAreOfValueType
        {
            get
            {
                var filteredProperties = SelectedProperties
                    .Where(property => property.PropertyInfo.PropertyType.IsValueType);

                return new InstancePropertyOfValueTypeSelector<TDeclaringType>(Instance, filteredProperties);
            }
        }

        /// <summary>
        /// Only select the properties that are not of reference type.
        /// </summary>
        public InstancePropertySelector<TDeclaringType> ThatAreOfReferenceType
        {
            get
            {
                var filteredProperties = SelectedProperties
                    .Where(property => !property.PropertyInfo.PropertyType.IsValueType);

                return CloneFiltered(filteredProperties);
            }
        }

    }
}
