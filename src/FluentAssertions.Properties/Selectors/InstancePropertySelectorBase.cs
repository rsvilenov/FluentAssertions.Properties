using FluentAssertions.Properties.Data;
using FluentAssertions.Properties.Extensions;
using FluentAssertions.Properties.Invocation;
using FluentAssertions.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentAssertions.Properties.Selectors
{
    public abstract class InstancePropertySelectorBase<TDeclaringType, TInstancePropertyInfo, TSelector>
        : IEnumerable<TInstancePropertyInfo>
        where TInstancePropertyInfo : InstancePropertyInfo<TDeclaringType>
        where TSelector : InstancePropertySelectorBase<TDeclaringType, TInstancePropertyInfo, TSelector>
    {
        protected IEnumerable<TInstancePropertyInfo> SelectedProperties { get; set; } = new List<TInstancePropertyInfo>();
        protected TDeclaringType Instance { get; set; }
        protected abstract TSelector CloneFiltered(IEnumerable<TInstancePropertyInfo> filteredProperties);

        protected InstancePropertySelectorBase()
        { 
        }
        
        protected InstancePropertySelectorBase(TDeclaringType instance, IEnumerable<TInstancePropertyInfo> instancePropertyInfos)
        {
            Instance = instance;
            SelectedProperties = instancePropertyInfos.ToList();
        }

        public TSelector ThatAreWritable
        {
            get
            {
                var filteredProperties = SelectedProperties
                    .Where(p => p.PropertyInfo.CanWrite);

                return CloneFiltered(filteredProperties);
            }
        }

        public TSelector ThatAreReadOnly
        {
            get
            {
                var filteredProperties = SelectedProperties
                    .Where(p => !p.PropertyInfo.CanWrite);

                return CloneFiltered(filteredProperties);
            }
        }

        public TSelector ThatAreVirtual
        {
            get
            {
                var filteredProperties = SelectedProperties
                    .Where(property => property.PropertyInfo.GetMethod.IsVirtual);

                return CloneFiltered(filteredProperties);
            }
        }

        public TSelector ThatAreNotVirtual
        {
            get
            {
                var filteredProperties = SelectedProperties
                    .Where(property => !property.PropertyInfo.GetMethod.IsVirtual);

                return CloneFiltered(filteredProperties); ;
            }
        }

        public TSelector ThatAreOfPrimitiveTypes
        {
            get
            {
                var filteredProperties = SelectedProperties
                    .Where(property => property.PropertyInfo.PropertyType.IsPrimitive);

                return CloneFiltered(filteredProperties);
            }
        }

        public TSelector ThatAreNotOfPrimitiveTypes
        {
            get
            {
                var filteredProperties = SelectedProperties
                    .Where(property => !property.PropertyInfo.PropertyType.IsPrimitive);

                return CloneFiltered(filteredProperties);
            }
        }

        public TSelector ThatAreNotInternal
        {
            get
            {
                var filteredProperties = SelectedProperties
                    .Where(property =>
                    {
                        var getMethod = property.PropertyInfo.GetGetMethod();
                        return !(getMethod == null || getMethod.IsFamily);
                    });

                return CloneFiltered(filteredProperties);
            }
        }

        public TSelector ThatAreNotInheritted
        {
            get
            {
                var filteredProperties = SelectedProperties
                    .Where(property => property.PropertyInfo.DeclaringType.Equals(property.InstanceType));

                return CloneFiltered(filteredProperties);
            }
        }

        public TSelector OfTypeMatching(Predicate<PropertyInfo> condition)
        {
            var filteredProperties = SelectedProperties
                .Where(p => condition(p.PropertyInfo));

            return CloneFiltered(filteredProperties);
        }

        public PropertyInvocationCollection<TDeclaringType, object> WhenCalledWithValuesFrom(TDeclaringType source)
        {
            var propertyInvocationInfos = new List<PropertyInvocationInfo<TDeclaringType, object>>();
            var propertyInvoker = InvocationContext.PropertyInvokerFactory.CreatePropertyInvoker<TDeclaringType>(source);
            
            foreach (PropertyInfo propInfo in typeof(TDeclaringType)
                .GetPublicOrInternalProperties()
                .Where(pi => SelectedProperties.Any(ipi => ipi.PropertyInfo.Name == pi.Name)))
            {
                object value = propertyInvoker.GetValue<object>(propInfo.Name);
                propertyInvocationInfos.Add(new PropertyInvocationInfo<TDeclaringType, object>(propInfo, value));
            }

            return new PropertyInvocationCollection<TDeclaringType, object>(Instance, propertyInvocationInfos);
        }

        /// <summary>
        /// Select return types of the properties
        /// </summary>
        public TypeSelector ReturnTypes()
        {
            var returnTypes = SelectedProperties.Select(property => property.PropertyInfo.PropertyType);

            return new TypeSelector(returnTypes);
        }

        /// <summary>
        /// The resulting <see cref="PropertyInfo"/> objects.
        /// </summary>
        public TInstancePropertyInfo[] ToArray()
        {
            return SelectedProperties.ToArray();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="System.Collections.Generic.IEnumerator{T}"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<TInstancePropertyInfo> GetEnumerator()
        {
            return SelectedProperties.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
