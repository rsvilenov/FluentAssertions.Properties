using FluentAssertions.Properties.Data;
using FluentAssertions.Properties.Invocation;
using System.Collections.Generic;
using System.Linq;

namespace FluentAssertions.Properties.Selectors
{
    public class InstancePropertyOfKnownTypeSelector<TDeclaringType, TProperty> :
        InstancePropertySelectorBase<TDeclaringType, InstancePropertyInfo<TDeclaringType, TProperty>, InstancePropertyOfKnownTypeSelector<TDeclaringType, TProperty>>
        
    {
        internal InstancePropertyOfKnownTypeSelector(TDeclaringType instance, IEnumerable<InstancePropertyInfo<TDeclaringType, TProperty>> instancePropertyInfos)
            : base(instance, instancePropertyInfos)
        {
        }

        public InstancePropertyOfKnownTypeSelector<TDeclaringType, TProperty> HavingValue(TProperty value)
        {
            var propertyInvoker = InvocationContext.PropertyInvokerFactory.CreatePropertyInvoker<TDeclaringType>(Instance);
            
            var filteredProperties = SelectedProperties
                .Where(p => Equals(propertyInvoker.GetValue(p.PropertyInfo.Name), value));

            return CloneFiltered(filteredProperties);
        }

        public PropertyInvocationCollection<TDeclaringType, TProperty> WhenCalledWith(TProperty value)
        {
            var propertyInvocationInfos = SelectedProperties
                .Select(ipi => new PropertyInvocationInfo<TDeclaringType, TProperty>(ipi.PropertyInfo, () => value));
            
            return new PropertyInvocationCollection<TDeclaringType, TProperty>(
                Instance,
                propertyInvocationInfos);
        }

        protected override InstancePropertyOfKnownTypeSelector<TDeclaringType, TProperty> CloneFiltered(IEnumerable<InstancePropertyInfo<TDeclaringType, TProperty>> filteredProperties)
        {
            return new InstancePropertyOfKnownTypeSelector<TDeclaringType, TProperty>(Instance, filteredProperties);
        }
    }
}
