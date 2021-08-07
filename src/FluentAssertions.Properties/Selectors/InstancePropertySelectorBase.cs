using FluentAssertions.Properties.Data;
using FluentAssertions.Types;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentAssertions.Properties.Selectors
{
    public abstract class InstancePropertySelectorBase<TDeclaringType, TInstancePropertyInfo> : IEnumerable<TInstancePropertyInfo>
        where TInstancePropertyInfo : InstancePropertyInfo<TDeclaringType>, new()
    {
        internal protected IEnumerable<TInstancePropertyInfo> SelectedProperties { get; set; } = new List<TInstancePropertyInfo>();
        internal protected TDeclaringType Instance { get; set; }

        public InstancePropertySelectorBase() { }
        internal InstancePropertySelectorBase(InstancePropertySelectorBase<TDeclaringType, TInstancePropertyInfo> instancePropertySelector)
        {
            Instance = instancePropertySelector.Instance;
            SelectedProperties = instancePropertySelector.SelectedProperties;
        }
        /// <summary>
        /// Only select the properties that have a public or internal getter.
        /// </summary>
        public InstancePropertySelectorBase<TDeclaringType, TInstancePropertyInfo> ThatArePublicOrInternal
        {
            get
            {
                SelectedProperties = SelectedProperties.Where(property =>
                {
                    MethodInfo getter = property.PropertyInfo.GetGetMethod(nonPublic: true);
                    return (getter != null) && (getter.IsPublic || getter.IsAssembly);
                });

                return this;
            }
        }

        public InstancePropertySelectorBase<TDeclaringType, TInstancePropertyInfo> ThatAreWritable
        {
            get
            {
                SelectedProperties = SelectedProperties.Where(property =>
                {
                    MethodInfo setter = property.PropertyInfo.GetSetMethod(nonPublic: true);
                    return (setter != null) && (setter.IsPublic || setter.IsAssembly);
                });

                return this;
            }
        }

        public InstancePropertySelectorBase<TDeclaringType, TInstancePropertyInfo> ThatAreReadOnly
        {
            get
            {
                SelectedProperties = SelectedProperties.Where(property =>
                {
                    MethodInfo setter = property.PropertyInfo.GetSetMethod(nonPublic: true);
                    return setter == null || (!setter.IsPublic && !setter.IsAssembly);
                });

                return this;
            }
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
