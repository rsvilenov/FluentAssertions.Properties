using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FluentAssertions.Properties.Data
{
    public class PropertyInvocationCollection<TDeclaringType, TProperty> : IEnumerable<InstancePropertyInfo<TDeclaringType, TProperty>>
    {
        public TProperty Value { get; }
        protected IEnumerable<InstancePropertyInfo<TDeclaringType, TProperty>> SelectedProperties { get; set; } = new List<InstancePropertyInfo<TDeclaringType, TProperty>>();
        public TDeclaringType Instance { get; }

        internal PropertyInvocationCollection(TDeclaringType instance, TProperty value, IEnumerable<InstancePropertyInfo<TDeclaringType, TProperty>> instancePropertyInfos)
        {
            Instance = instance;
            Value = value;
            SelectedProperties = instancePropertyInfos.ToList(); // copy the collection
        }
        /// <summary>
        /// The resulting <see cref="PropertyInfo"/> objects.
        /// </summary>
        public InstancePropertyInfo<TDeclaringType, TProperty>[] ToArray()
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
        public IEnumerator<InstancePropertyInfo<TDeclaringType, TProperty>> GetEnumerator()
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
