using FluentAssertions.Properties.Data;
using FluentAssertions.Properties.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentAssertions.Properties.Selectors
{
    public class InstancePropertySelector<TDeclaringType> 
        :  InstancePropertySelectorBase<TDeclaringType, InstancePropertyInfo<TDeclaringType>, InstancePropertySelector<TDeclaringType>>
    {
        internal InstancePropertySelector(TDeclaringType instance, IEnumerable<string> propertyNames = null)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(paramName: nameof(instance));
            }

            Instance = instance;

            var properties = new List<InstancePropertyInfo<TDeclaringType>>();

            Type type = instance.GetType();
            foreach (PropertyInfo propInfo in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if ((propertyNames == null || propertyNames.Contains(propInfo.Name))
                    && propInfo.HasPublicOrInternalGetter())
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
        /// Only select the properties that return the specified type
        /// </summary>
        public InstancePropertyWithKnownTypeSelector<TDeclaringType, TProperty> OfType<TProperty>()
        {
            var selectedProperties = SelectedProperties
                .Where(property => property.PropertyInfo.PropertyType == typeof(TProperty))
                .Select(p => new InstancePropertyInfo<TDeclaringType, TProperty>(p));

            return new InstancePropertyWithKnownTypeSelector<TDeclaringType, TProperty>(Instance, selectedProperties);
        }

        /// <summary>
        /// Only select the properties that do not return the specified type
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

        public InstancePropertyOfValueTypeSelector<TDeclaringType> ThatAreOfValueType
        {
            get
            {
                var filteredProperties = SelectedProperties
                    .Where(property => property.PropertyInfo.PropertyType.IsValueType);

                return new InstancePropertyOfValueTypeSelector<TDeclaringType>(Instance, filteredProperties);
            }
        }

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
