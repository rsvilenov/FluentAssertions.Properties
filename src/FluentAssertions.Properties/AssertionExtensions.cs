using FluentAssertions.Properties.Assertions;
using FluentAssertions.Properties.Data;
using FluentAssertions.Properties.Selectors;
using System.Diagnostics;

namespace FluentAssertions.Properties
{
    /// <summary>
    /// Contains extension methods for custom assertions in unit tests.
    /// </summary>
    [DebuggerNonUserCode]
    public static class AssertionExtensions
    {
        /// <summary>
        /// Returns an assertion object that can be used to assert
        /// a single property selected by the selectors in <see cref="ObjectExtensions"/>
        /// </summary>
        public static InstancePropertyInfoAssertions<TDeclaringType> Should<TDeclaringType>(this InstancePropertyInfo<TDeclaringType> actualValue)
        {
            return new InstancePropertyInfoAssertions<TDeclaringType>(actualValue);
        }

        /// <summary>
        /// Returns an assertion object that can be used to assert
        /// properties selected by the selectors in <see cref="ObjectExtensions"/>
        /// </summary>
        public static InstancePropertySelectorAssertions<TDeclaringType, InstancePropertyInfo<TDeclaringType, TProperty>, TSelector> Should<TDeclaringType, TProperty, TSelector>(this InstancePropertySelectorBase<TDeclaringType, InstancePropertyInfo<TDeclaringType, TProperty>, TSelector> actualValue)
            where TSelector : InstancePropertySelectorBase<TDeclaringType, InstancePropertyInfo<TDeclaringType, TProperty>, TSelector>
        {
            return new InstancePropertySelectorAssertions<TDeclaringType, InstancePropertyInfo<TDeclaringType, TProperty>, TSelector>(actualValue);
        }

        /// <summary>
        /// Returns an assertion object that can be used to assert
        /// properties selected by the selectors in <see cref="ObjectExtensions"/>
        /// </summary>
        public static InstancePropertySelectorAssertions<TDeclaringType, InstancePropertyInfo<TDeclaringType>, TSelector> Should<TDeclaringType, TSelector>(this InstancePropertySelectorBase<TDeclaringType, InstancePropertyInfo<TDeclaringType>, TSelector> actualValue)
            where TSelector : InstancePropertySelectorBase<TDeclaringType, InstancePropertyInfo<TDeclaringType>, TSelector>
        {
            return new InstancePropertySelectorAssertions<TDeclaringType, InstancePropertyInfo<TDeclaringType>, TSelector>(actualValue);
        }

        /// <summary>
        /// Returns an assertion object that can be used to assert the behavior of
        /// properties selected by the selectors in <see cref="ObjectExtensions"/>,
        /// which have been prepared for invocation with the methods like 
        /// <see cref="InstancePropertySelectorBase{TDeclaringType, TInstancePropertyInfo, TSelector}.WhenCalledWithValuesFrom(TDeclaringType)"/>
        /// </summary>
        public static PropertyInvocationCollectionAssertions<TDeclaringType, TProperty> Should<TDeclaringType, TProperty>(this PropertyInvocationCollection<TDeclaringType, TProperty> actualValue)
        {
            return new PropertyInvocationCollectionAssertions<TDeclaringType, TProperty>(actualValue);
        }
    }
}
