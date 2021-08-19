using FluentAssertions.Properties.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentAssertions.Properties.Selectors
{
    public class InstancePropertyWithKnownTypeSelector<TDeclaringType, TProperty> :
        InstancePropertySelectorBase<TDeclaringType, InstancePropertyInfo<TDeclaringType, TProperty>>
        
    {
        internal InstancePropertyWithKnownTypeSelector(TDeclaringType instance, InstancePropertyInfo<TDeclaringType, TProperty> instancePropertyInfo)
               : this(instance, new[] { instancePropertyInfo })
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InstancePropertyInfoSelector"/> class.
        /// </summary>
        /// <param name="types">The types from which to select properties.</param>
        /// <exception cref="ArgumentNullException"><paramref name="types"/> is <c>null</c>.</exception>
        internal InstancePropertyWithKnownTypeSelector(TDeclaringType instance, IEnumerable<InstancePropertyInfo<TDeclaringType, TProperty>> instancePropertyInfos)
        {
            Instance = instance;
            SelectedProperties = instancePropertyInfos;
        }

        public InstancePropertyWithKnownTypeSelector<TDeclaringType, TProperty> HavingValue(TProperty value)
        {
            SelectedProperties = SelectedProperties
                .Where(property => property.PropertyValue.Equals(value));

            return this;
        }


        /// <summary>
        /// Only select the properties that return the specified type
        /// </summary>
        public PropertyInvocationCollection<TDeclaringType, TProperty> WhenCalledWith(TProperty value)
        {
            return new PropertyInvocationCollection<TDeclaringType, TProperty>(Instance, value, SelectedProperties);
        }
    }
}
