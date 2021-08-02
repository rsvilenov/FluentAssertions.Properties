using FluentAssertions.Properties.Objects;
using FluentAssertions.Properties.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Reflection;

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
        public static InstancePropertyInfoSelectorAssertions<TDeclaringType, TProperty> Should<TDeclaringType, TProperty>(this InstancePropertyInfoSelector<TDeclaringType, TProperty> actualValue)
        {
            return new InstancePropertyInfoSelectorAssertions<TDeclaringType, TProperty>(actualValue);
        }

        [Pure]
        public static InstanceWithValuePropertyInfoSelectorAssertions<TDeclaringType, TProperty> Should<TDeclaringType, TProperty>(this InstanceWithValuePropertyInfoSelector<TDeclaringType, TProperty> actualValue)
        {
            return new InstanceWithValuePropertyInfoSelectorAssertions<TDeclaringType, TProperty>(actualValue);
        }
    }
}
