using FluentAssertions.Properties.Extensions;
using FluentAssertions.Properties.Selectors;
using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace FluentAssertions.Properties
{
    public static class ObjectExtensions
    {
        public static InstancePropertySelector<TDeclaringType> Properties<TDeclaringType>(this TDeclaringType instance, params Expression<Func<TDeclaringType, object>>[] properties)
        {
            var propertyNames = properties.Any()
                ? properties.Select(property => property.GetMemberName())
                : null;

            return new InstancePropertySelector<TDeclaringType>(instance, propertyNames);
        }

        public static InstancePropertyWithKnownTypeSelector<TDeclaringType, TProperty> Properties<TDeclaringType, TProperty>(this TDeclaringType instance, params Expression<Func<TDeclaringType, TProperty>>[] properties)
        {
            var propertyNames = properties.Any()
                ? properties.Select(property => property.GetMemberName())
                : null;

            var selector = new InstancePropertySelector<TDeclaringType>(instance, propertyNames);
            return selector.OfType<TProperty>();
        }
    }
}
