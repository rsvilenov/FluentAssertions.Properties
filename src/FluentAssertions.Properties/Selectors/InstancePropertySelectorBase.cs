using FluentAssertions.Properties.Common;
using FluentAssertions.Properties.Data;
using FluentAssertions.Properties.Extensions;
using FluentAssertions.Properties.Invocation;
using FluentAssertions.Types;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentAssertions.Properties.Selectors
{
    /// <summary>
    /// Allows for fluent selection of class properties of an instance and setting them up for assertion.
    /// <typeparam name="TDeclaringType">The type of the instance.</typeparam>
    /// <typeparam name="TInstancePropertyInfo">The type of the <see cref="InstancePropertyInfo{TDeclaringType}"/> for the selected properties.</typeparam>
    /// <typeparam name="TSelector">The actual type of the selector.</typeparam>
    /// </summary>
    public abstract class InstancePropertySelectorBase<TDeclaringType, TInstancePropertyInfo, TSelector>
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

        /// <summary>
        /// Only select the properties that have a setter.
        /// </summary>
        public TSelector ThatAreWritable
        {
            get
            {
                var filteredProperties = SelectedProperties
                    .Where(p => p.PropertyInfo.CanWrite);

                return CloneFiltered(filteredProperties);
            }
        }

        /// <summary>
        /// Only select the properties that do not have a setter.
        /// </summary>
        public TSelector ThatAreReadOnly
        {
            get
            {
                var filteredProperties = SelectedProperties
                    .Where(p => !p.PropertyInfo.CanWrite);

                return CloneFiltered(filteredProperties);
            }
        }

#if NET5_0_OR_GREATER

        /// <summary>
        /// Only select the properties that have an init only setter.
        /// </summary>
        public TSelector ThatAreInitOnly
        {
            get
            {
                var filteredProperties = SelectedProperties
                    .Where(p => p.PropertyInfo.IsInitOnly());

                return CloneFiltered(filteredProperties);
            }
        }

#endif

        /// <summary>
        /// Only select the properties that are marked as virtual.
        /// </summary>
        public TSelector ThatAreVirtual
        {
            get
            {
                var filteredProperties = SelectedProperties
                    .Where(property => property.PropertyInfo.GetMethod.IsVirtual);

                return CloneFiltered(filteredProperties);
            }
        }

        /// <summary>
        /// Only select the properties that are not marked as virtual.
        /// </summary>
        public TSelector ThatAreNotVirtual
        {
            get
            {
                var filteredProperties = SelectedProperties
                    .Where(property => !property.PropertyInfo.GetMethod.IsVirtual);

                return CloneFiltered(filteredProperties); ;
            }
        }

        /// <summary>
        /// Only select the internal properties.
        /// </summary>
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

        /// <summary>
        /// Only select the properties that are not internal.
        /// </summary>
        public TSelector ThatAreNotInheritted
        {
            get
            {
                var filteredProperties = SelectedProperties
                    .Where(property => property.PropertyInfo.DeclaringType.Equals(property.InstanceType));

                return CloneFiltered(filteredProperties);
            }
        }

        /// <summary>
        /// Prepares the selected properties to be asserted by associating them with the values from a source object of the same type.
        /// This method does not assign the value to the properties. It just associates the value
        /// with the property internally in the library, so that the assert step knows how to assert them.        
        /// </summary>
        /// <param name="source">The source object whose values to be used.</param>
        /// <returns>An assertable property invocation collection.</returns>
        public PropertyInvocationCollection<TDeclaringType, object> WhenCalledWithValuesFrom(TDeclaringType source)
        {
            Guard.ThrowIfArgumentIsNull(source, nameof(source));

            var propertyInvocationInfos = new List<PropertyInvocationInfo<TDeclaringType, object>>();
            var propertyInvoker = InvocationContext.PropertyInvokerFactory.CreatePropertyInvoker<TDeclaringType, object>(source);
            
            foreach (PropertyInfo propInfo in typeof(TDeclaringType)
                .GetPublicOrInternalProperties()
                .Where(pi => SelectedProperties.Any(ipi => ipi.PropertyInfo.Name == pi.Name)))
            {
                var valueDelegate = () => propertyInvoker.GetValue(propInfo.Name);
                propertyInvocationInfos.Add(new PropertyInvocationInfo<TDeclaringType, object>(propInfo, valueDelegate));
            }

            return new PropertyInvocationCollection<TDeclaringType, object>(Instance, propertyInvocationInfos);
        }

        /// <summary>
        /// Select return types of the properties.
        /// </summary>
        public TypeSelector ReturnTypes()
        {
            var returnTypes = SelectedProperties.Select(property => property.PropertyInfo.PropertyType);

            return new TypeSelector(returnTypes);
        }

        /// <summary>
        /// Returns the number of selected properties.
        /// </summary>
        public int Count()
        {
            return SelectedProperties.Count();
        }
        internal TInstancePropertyInfo[] GetSelection()
        {
            return SelectedProperties.ToArray();
        }
    }
}
