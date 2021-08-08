using System;

namespace FluentAssertions.Properties.Selectors
{
    internal static class TypeExtensions
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

    }
}
