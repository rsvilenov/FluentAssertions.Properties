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
            if (CheckIfTypeIsNullableValueType(type))
            {
                var valueProperty = type.GetProperty("Value");
                type = valueProperty.PropertyType;
            }

            return type;
        }

        public static bool CheckIfTypeIsNullableValueType(this Type type)
        {
            return 
                type.IsGenericType 
                && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public static IEnumerable<PropertyInfo> GetPublicOrInternalProperties(this Type type)
        {
            return type
                .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(propertyInfo =>
                {
                    MethodInfo getter = propertyInfo.GetGetMethod(nonPublic: true);
                    return (getter != null) && (getter.IsPublic || getter.IsAssembly);
                });
        }

    }
}