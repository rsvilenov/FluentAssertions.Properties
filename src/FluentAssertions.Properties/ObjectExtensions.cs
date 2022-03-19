using FluentAssertions.Properties.Common;
using FluentAssertions.Properties.Extensions;
using FluentAssertions.Properties.Selectors;
using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace FluentAssertions.Properties
{
    /// <summary>
    /// Extension methods for getting property selectors for an instance.
    /// </summary>
    [DebuggerNonUserCode]
    public static class ObjectExtensions
    {
        /// <summary>
        /// Returns a property selector for the current instance.
        /// </summary>
        /// <typeparam name="TDeclaringType">The type of the instance.</typeparam>
        /// <param name="instance">The instance of the type whose property will be tested.</param>
        /// <param name="properties">An expression, which specifies the properties to be selected.</param>
        /// <exception cref="ArgumentNullException"><paramref name="instance"/> is <c>null</c>.</exception>
        public static InstancePropertySelector<TDeclaringType> Properties<TDeclaringType>(this TDeclaringType instance, params Expression<Func<TDeclaringType, object>>[] properties)
        {
            Guard.ThrowIfArgumentIsNull(instance, nameof(instance));

            var propertyNames = properties.Any()
                ? properties.Select(property => property.GetMemberName())
                : null;

            return new InstancePropertySelector<TDeclaringType>(instance, propertyNames);
        }

        /// <summary>
        /// Returns a type-specific property selector for the current instance, when all selected properties are of the same type.
        /// </summary>
        /// <typeparam name="TDeclaringType">The type of the instance.</typeparam>
        /// <typeparam name="TProperty">The type of the selected properties.</typeparam>
        /// <param name="instance">The instance of the type whose property will be tested.</param>
        /// <param name="properties">An expression, which specifies the properties to be selected.</param>
        /// <exception cref="ArgumentNullException"><paramref name="instance"/> is <c>null</c>.</exception>
        public static InstancePropertyOfKnownTypeSelector<TDeclaringType, TProperty> Properties<TDeclaringType, TProperty>(this TDeclaringType instance, params Expression<Func<TDeclaringType, TProperty>>[] properties)
        {
            Guard.ThrowIfArgumentIsNull(instance, nameof(instance));

            var propertyNames = properties.Any()
                ? properties.Select(property => property.GetMemberName())
                : null;

            var selector = new InstancePropertySelector<TDeclaringType>(instance, propertyNames);
            return selector.ExactlyOfType<TProperty>();
        }
    }
}
