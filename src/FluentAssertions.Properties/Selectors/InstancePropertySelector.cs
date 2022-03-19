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
        /// Only select the properties that can be cast to the specified type.
        /// </summary>
        public InstancePropertyOfCategorySelector<TDeclaringType, TProperty> OfType<TProperty>()
        {
            var selectedProperties = SelectedProperties
                .Where(property => typeof(TProperty).IsAssignableFrom(property.PropertyInfo.PropertyType))
                .Select(p => new InstancePropertyInfo<TDeclaringType, TProperty>(p));
            
            return new InstancePropertyOfCategorySelector<TDeclaringType, TProperty>(Instance, selectedProperties);
        }

        /// <summary>
        /// Only select the properties whose type is exactly the specified one.
        /// </summary>
        public InstancePropertyOfKnownTypeSelector<TDeclaringType, TProperty> ExactlyOfType<TProperty>()
        {
            var selectedProperties = SelectedProperties
                .Where(property => property.PropertyInfo.PropertyType == typeof(TProperty))
                .Select(p => new InstancePropertyInfo<TDeclaringType, TProperty>(p));

            return new InstancePropertyOfKnownTypeSelector<TDeclaringType, TProperty>(Instance, selectedProperties);
        }

        /// <summary>
        /// Only select the properties that are cannot be cast to the specified type
        /// </summary>
        public InstancePropertySelector<TDeclaringType> NotOfType<TProperty>()
        {
            var filteredProperties = SelectedProperties
                .Where(property => !typeof(TProperty).IsAssignableFrom(property.PropertyInfo.PropertyType));

            return CloneFiltered(filteredProperties);
        }

        /// <summary>
        /// Only select the properties whose <see cref="PropertyInfo"/> matches a given predicate.
        /// </summary>
        /// <param name="condition">The filter condition for the properties.</param>
        public InstancePropertyOfCategorySelector<TDeclaringType> OfTypeMatching(Predicate<PropertyInfo> condition)
        {
            var selectedProperties = SelectedProperties
                .Where(p => condition(p.PropertyInfo));

            return new InstancePropertyOfCategorySelector<TDeclaringType>(Instance, selectedProperties);
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
        public InstancePropertyOfCategorySelector<TDeclaringType> ThatAreOfReferenceType
        {
            get
            {
                var selectedProperties = SelectedProperties
                    .Where(property => !property.PropertyInfo.PropertyType.IsValueType);

                return new InstancePropertyOfCategorySelector<TDeclaringType>(Instance, selectedProperties);
            }
        }



        /// <summary>
        /// Only select the properties whose types are among the primitive ones.
        /// </summary>
        public InstancePropertyOfCategorySelector<TDeclaringType> ThatAreOfPrimitiveTypes
        {
            get
            {
                var selectedProperties = SelectedProperties
                    .Where(property => property.PropertyInfo.PropertyType.IsPrimitive);

                return new InstancePropertyOfCategorySelector<TDeclaringType>(Instance, selectedProperties);
            }
        }

        /// <summary>
        /// Only select the properties whose types are not among the primitive ones.
        /// </summary>
        public InstancePropertyOfCategorySelector<TDeclaringType> ThatAreNotOfPrimitiveTypes
        {
            get
            {
                var selectedProperties = SelectedProperties
                    .Where(property => !property.PropertyInfo.PropertyType.IsPrimitive);

                return new InstancePropertyOfCategorySelector<TDeclaringType>(Instance, selectedProperties);
            }
        }

        protected override InstancePropertySelector<TDeclaringType> CloneFiltered(IEnumerable<InstancePropertyInfo<TDeclaringType>> filteredProperties)
        {
            return new InstancePropertySelector<TDeclaringType>(Instance, filteredProperties);
        }

    }
}
