using FluentAssertions.Properties.Data;
using FluentAssertions.Properties.Assertions;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Reflection;
using FluentAssertions.Properties.Selectors;

namespace FluentAssertions.Properties
{
    public static class AssertionExtensions
    {
        /// <summary>
        /// Returns an <see cref="BooleanAssertions"/> object that can be used to assert the
        /// current <see cref="bool"/>.
        /// </summary>
        [Pure]
        public static PropertyInfoAssertions Should(this PropertyInfo actualValue)
        {
            return new PropertyInfoAssertions(actualValue);
        }

        [Pure]
        public static InstancePropertySelectorAssertions<TDeclaringType, InstancePropertyInfo<TDeclaringType, TProperty>> Should<TDeclaringType, TProperty>(this InstancePropertySelectorBase<TDeclaringType, InstancePropertyInfo<TDeclaringType, TProperty>> actualValue)
        {
            return new InstancePropertySelectorAssertions<TDeclaringType, InstancePropertyInfo<TDeclaringType, TProperty>>(actualValue);
        }

        [Pure]
        public static InstancePropertySelectorAssertions<TDeclaringType, InstancePropertyInfo<TDeclaringType>> Should<TDeclaringType>(this InstancePropertySelectorBase<TDeclaringType, InstancePropertyInfo<TDeclaringType>> actualValue)
        {
            return new InstancePropertySelectorAssertions<TDeclaringType, InstancePropertyInfo<TDeclaringType>>(actualValue);
        }

        [Pure]
        public static PropertyInvocationCollectionAssertions<TDeclaringType, TProperty> Should<TDeclaringType, TProperty>(this PropertyInvocationCollection<TDeclaringType, TProperty> actualValue)
        {
            return new PropertyInvocationCollectionAssertions<TDeclaringType, TProperty>(actualValue);
        }
    }
}
