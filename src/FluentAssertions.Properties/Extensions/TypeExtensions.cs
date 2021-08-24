using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentAssertions.Properties.Extensions
{
    public static class TypeExtensions
    {
        public static Type GetActualTypeIfNullable(this Type type)
        {
            if (IsNullableValueType(type))
            {
                var valueProperty = type.GetProperty("Value");
                type = valueProperty.PropertyType;
            }

            return type;
        }

        public static bool IsNullableValueType(this Type type)
        {
            return Nullable.GetUnderlyingType(type) != null;
        }

        public static IEnumerable<PropertyInfo> GetPublicOrInternalProperties(this Type type)
        {
            return type
                .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(propertyInfo => propertyInfo.HasPublicOrInternalGetter());
        }

    }
}