using FluentAssertions.Properties.Data;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace FluentAssertions.Properties.Selectors
{
    /// <summary>
    /// Allows for fluent selection of class properties of a type category.
    /// <typeparamref name="TDeclaringType">The type of the instance.</typeparamref>
    /// </summary>
    [DebuggerNonUserCode]
    public class InstancePropertyOfCategorySelector<TDeclaringType> :
        InstancePropertySelectorBase<TDeclaringType, InstancePropertyInfo<TDeclaringType>, InstancePropertyOfCategorySelector<TDeclaringType>>

    {
        internal InstancePropertyOfCategorySelector(TDeclaringType instance, IEnumerable<InstancePropertyInfo<TDeclaringType>> instancePropertyInfos)
            : base(instance, instancePropertyInfos)
        {
        }

        [ExcludeFromCodeCoverage]
        protected override InstancePropertyOfCategorySelector<TDeclaringType> CloneFiltered(IEnumerable<InstancePropertyInfo<TDeclaringType>> filteredProperties)
        {
            throw new System.NotImplementedException();
        }
    }
    
    /// <summary>
    /// Allows for fluent selection of class properties of a type category.
    /// <typeparamref name="TDeclaringType">The type of the instance.</typeparamref>
    /// <typeparamref name="TProperty">The type of the selected properties.</typeparamref>
    /// </summary>
    [DebuggerNonUserCode]
    public class InstancePropertyOfCategorySelector<TDeclaringType, TProperty> :
        InstancePropertySelectorBase<TDeclaringType, InstancePropertyInfo<TDeclaringType, TProperty>, InstancePropertyOfCategorySelector<TDeclaringType, TProperty>>
        
    {
        internal InstancePropertyOfCategorySelector(TDeclaringType instance, IEnumerable<InstancePropertyInfo<TDeclaringType, TProperty>> instancePropertyInfos)
            : base(instance, instancePropertyInfos)
        {
        }

        [ExcludeFromCodeCoverage]
        protected override InstancePropertyOfCategorySelector<TDeclaringType, TProperty> CloneFiltered(IEnumerable<InstancePropertyInfo<TDeclaringType, TProperty>> filteredProperties)
        {
            throw new System.NotImplementedException();
        }
    }
}
