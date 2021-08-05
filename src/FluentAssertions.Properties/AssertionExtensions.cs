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

        ///// <summary>
        ///// Returns an <see cref="BooleanAssertions"/> object that can be used to assert the
        ///// current <see cref="bool"/>.
        ///// </summary>
        //[Pure]
        //public static InstancePropertyInfoAssertions Should(this InstancePropertyInfo actualValue)
        //{
        //    return new InstancePropertyInfoAssertions(actualValue);
        //}

        //[Pure]
        //public static InstancePropertyInfoSelectorAssertions Should(this InstancePropertyInfoSelector actualValue)
        //{
        //    return new InstancePropertyInfoSelectorAssertions(actualValue);
        //}

        [Pure]
        public static InstancePropertySelectorAssertions<TDeclaringType, TProperty> Should<TDeclaringType, TProperty>(this InstancePropertyWithKnownTypeSelector<TDeclaringType, TProperty> actualValue)
        {
            return new InstancePropertySelectorAssertions<TDeclaringType, TProperty>(actualValue);
        }

        [Pure]
        public static PropertyInvocationCollectionAssertions<TDeclaringType, TProperty> Should<TDeclaringType, TProperty>(this PropertyInvocationCollection<TDeclaringType, TProperty> actualValue)
        {
            return new PropertyInvocationCollectionAssertions<TDeclaringType, TProperty>(actualValue);
        }
    }
}
