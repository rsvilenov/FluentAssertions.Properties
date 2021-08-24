using FluentAssertions.Properties.Data;
using FluentAssertions.Properties.Extensions;
using FluentAssertions.Properties.Invocation;
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

        internal InstancePropertyWithKnownTypeSelector(TDeclaringType instance, IEnumerable<InstancePropertyInfo<TDeclaringType, TProperty>> instancePropertyInfos)
        {
            Instance = instance;
            SelectedProperties = instancePropertyInfos;
        }

        public InstancePropertyWithKnownTypeSelector<TDeclaringType, TProperty> HavingValue(TProperty value)
        {
            var propertyInvoker = InvocationContext.PropertyInvokerFactory.CreatePropertyInvoker<TDeclaringType>(Instance);
            
            SelectedProperties = SelectedProperties
                .Where(p => p.PropertyInfo.HasPublicOrInternalGetter()
                    && propertyInvoker.GetValue<TProperty>(p.PropertyInfo.Name).Equals(value));

            return this;
        }

        public PropertyInvocationCollection<TDeclaringType, TProperty> WhenCalledWith(TProperty value)
        {
            var propertyInvocationInfos = SelectedProperties
                .Select(ipi => new PropertyInvocationInfo<TDeclaringType, TProperty>(ipi.PropertyInfo, value));
            
            return new PropertyInvocationCollection<TDeclaringType, TProperty>(
                Instance,
                propertyInvocationInfos);
        }
    }
}
