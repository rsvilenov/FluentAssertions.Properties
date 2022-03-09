using FluentAssertions.Properties.Common;
using FluentAssertions.Types;
using System;
using System.Linq;

namespace FluentAssertions.Properties
{
    /// <summary>
    /// Extension methods for getting property selectors for a type.
    /// </summary>
    public static class TypeExtensions
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
    }
}
