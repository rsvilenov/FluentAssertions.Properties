using FluentAssertions.Properties.Data;
using FluentAssertions.Properties.Extensions;
using FluentAssertions.Properties.Invocation;
using System.Collections.Generic;
using System.Linq;

namespace FluentAssertions.Properties.Selectors
{
    public class InstancePropertyWithKnownTypeSelector<TDeclaringType, TProperty> :
        InstancePropertySelectorBase<TDeclaringType, InstancePropertyInfo<TDeclaringType, TProperty>, InstancePropertyWithKnownTypeSelector<TDeclaringType, TProperty>>
        
    {
        internal InstancePropertyWithKnownTypeSelector(TDeclaringType instance, IEnumerable<InstancePropertyInfo<TDeclaringType, TProperty>> instancePropertyInfos)
            : base(instance, instancePropertyInfos)
        {
        }

        public InstancePropertyWithKnownTypeSelector<TDeclaringType, TProperty> HavingValue(TProperty value)
        {
            var propertyInvoker = InvocationContext.PropertyInvokerFactory.CreatePropertyInvoker<TDeclaringType>(Instance);
            
            var filteredProperties = SelectedProperties
                .Where(p => p.PropertyInfo.HasPublicOrInternalGetter()
                    && propertyInvoker.GetValue<TProperty>(p.PropertyInfo.Name).Equals(value));

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

        protected override InstancePropertyWithKnownTypeSelector<TDeclaringType, TProperty> CloneFiltered(IEnumerable<InstancePropertyInfo<TDeclaringType, TProperty>> filteredProperties)
        {
            return new InstancePropertyWithKnownTypeSelector<TDeclaringType, TProperty>(Instance, filteredProperties);
        }
    }
}
