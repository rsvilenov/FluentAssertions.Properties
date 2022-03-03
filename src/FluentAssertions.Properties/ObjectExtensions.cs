using FluentAssertions.Properties.Common;
using FluentAssertions.Properties.Extensions;
using FluentAssertions.Properties.Selectors;
using FluentAssertions.Types;
using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace FluentAssertions.Properties
{
    [DebuggerNonUserCode]
    public static class ObjectExtensions
    {
        /// <summary>
        /// Returns a property selector for the current <see cref="Type"/>. 
        /// This method directs the call to the original FluentAssertions library.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> is <c>null</c>.</exception>
        public static PropertyInfoSelector Properties(this Type type)
        {
            return new PropertyInfoSelector(type);
        }

        /// <summary>
        /// Returns a property selector for the current <see cref="Type"/>.
        /// This method directs the call to the original FluentAssertions library.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="typeSelector"/> is <c>null</c>.</exception>
        public static PropertyInfoSelector Properties(this TypeSelector typeSelector)
        {
            Guard.ThrowIfArgumentIsNull(typeSelector, nameof(typeSelector));

            return new PropertyInfoSelector(typeSelector.ToList());
        }

        public static InstancePropertySelector<TDeclaringType> Properties<TDeclaringType>(this TDeclaringType instance, params Expression<Func<TDeclaringType, object>>[] properties)
        {
            Guard.ThrowIfArgumentIsNull(instance, nameof(instance));

            var propertyNames = properties.Any()
                ? properties.Select(property => property.GetMemberName())
                : null;

            return new InstancePropertySelector<TDeclaringType>(instance, propertyNames);
        }

        public static InstancePropertyOfKnownTypeSelector<TDeclaringType, TProperty> Properties<TDeclaringType, TProperty>(this TDeclaringType instance, params Expression<Func<TDeclaringType, TProperty>>[] properties)
        {
            Guard.ThrowIfArgumentIsNull(instance, nameof(instance));

            var propertyNames = properties.Any()
                ? properties.Select(property => property.GetMemberName())
                : null;

            var selector = new InstancePropertySelector<TDeclaringType>(instance, propertyNames);
            return selector.OfType<TProperty>();
        }
    }
}
