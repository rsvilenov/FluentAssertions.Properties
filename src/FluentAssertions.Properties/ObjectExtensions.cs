﻿using FluentAssertions.Properties.Objects;
using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;

namespace FluentAssertions.Properties
{
    public static class ObjectExtensions
    {
        public static InstancePropertyInfoSelector<TDeclaringType> Properties<TDeclaringType>(this TDeclaringType instance, params Expression<Func<TDeclaringType, object>>[] properties)
        {
            var propertyNames = properties.Any()
                ? properties.Select(property => GetMemberName(property))
                : null;

            return new InstancePropertyInfoSelector<TDeclaringType>(instance, propertyNames);
        }

        public static InstancePropertyInfoSelector<TDeclaringType, TProperty> Properties<TDeclaringType, TProperty>(this TDeclaringType instance, params Expression<Func<TDeclaringType, TProperty>>[] properties)
        {
            var propertyNames = properties.Any()
                ? properties.Select(property => GetMemberName(property))
                : null;

            var selector = new InstancePropertyInfoSelector<TDeclaringType>(instance, propertyNames);
            return selector.OfType<TProperty>();
        }

        private static string GetMemberName<T>(this Expression<T> expression)
        {
            if (expression.Body is MemberExpression m)
            {
                return m.Member.Name;
            }

            if (expression.Body is UnaryExpression u
                && u.Operand is MemberExpression me)
            {
                return me.Member.Name;
            }

            throw new NotSupportedException($"Unsupported expression: {expression.GetType()}");
        }
    }
}
