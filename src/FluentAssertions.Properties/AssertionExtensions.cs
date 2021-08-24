using FluentAssertions.Properties.Assertions;
using FluentAssertions.Properties.Data;
using FluentAssertions.Properties.Selectors;
using System.Diagnostics.Contracts;

namespace FluentAssertions.Properties
{
    public static class AssertionExtensions
    {
        [Pure]
        public static InstancePropertyInfoAssertions<TDeclaringType> Should<TDeclaringType>(this InstancePropertyInfo<TDeclaringType> actualValue)
        {
            return new InstancePropertyInfoAssertions<TDeclaringType>(actualValue);
        }

        [Pure]
        public static InstancePropertySelectorAssertions<TDeclaringType, InstancePropertyInfo<TDeclaringType, TProperty>, TSelector> Should<TDeclaringType, TProperty, TSelector>(this InstancePropertySelectorBase<TDeclaringType, InstancePropertyInfo<TDeclaringType, TProperty>, TSelector> actualValue)
            where TSelector : InstancePropertySelectorBase<TDeclaringType, InstancePropertyInfo<TDeclaringType, TProperty>, TSelector>
        {
            return new InstancePropertySelectorAssertions<TDeclaringType, InstancePropertyInfo<TDeclaringType, TProperty>, TSelector>(actualValue);
        }

        [Pure]
        public static InstancePropertySelectorAssertions<TDeclaringType, InstancePropertyInfo<TDeclaringType>, TSelector> Should<TDeclaringType, TSelector>(this InstancePropertySelectorBase<TDeclaringType, InstancePropertyInfo<TDeclaringType>, TSelector> actualValue)
            where TSelector : InstancePropertySelectorBase<TDeclaringType, InstancePropertyInfo<TDeclaringType>, TSelector>
        {
            return new InstancePropertySelectorAssertions<TDeclaringType, InstancePropertyInfo<TDeclaringType>, TSelector>(actualValue);
        }

        public static PropertyInvocationCollectionAssertions<TDeclaringType, TProperty> Should<TDeclaringType, TProperty>(this PropertyInvocationCollection<TDeclaringType, TProperty> actualValue)
        {
            return new PropertyInvocationCollectionAssertions<TDeclaringType, TProperty>(actualValue);
        }
    }
}
