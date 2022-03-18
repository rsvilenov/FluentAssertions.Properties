using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FluentAssertions.Properties.Data
{
    public class PropertyInvocationCollection<TDeclaringType, TProperty> : IEnumerable<PropertyInvocationInfo<TDeclaringType, TProperty>>
    {
        protected internal IEnumerable<PropertyInvocationInfo<TDeclaringType, TProperty>> SelectedProperties { get; set; } = new List<PropertyInvocationInfo<TDeclaringType, TProperty>>();
        internal TDeclaringType Instance { get; }

        internal PropertyInvocationCollection(TDeclaringType instance, IEnumerable<PropertyInvocationInfo<TDeclaringType, TProperty>> instancePropertyInfos)
        {
            Instance = instance;
            SelectedProperties = instancePropertyInfos.ToList(); // copy the collection
        }

        public IEnumerator<PropertyInvocationInfo<TDeclaringType, TProperty>> GetEnumerator()
        {
            return SelectedProperties.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
