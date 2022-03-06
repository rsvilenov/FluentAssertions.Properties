using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace FluentAssertions.Properties.Extensions
{
    internal static class PropertyInfoExtensions
    {   
        public static bool HasPublicOrInternalGetter(this PropertyInfo propertyInfo)
        {
            MethodInfo getter = propertyInfo.GetGetMethod(nonPublic: true);
            return (getter != null) 
                && (getter.IsPublic || getter.IsAssembly);
        }

#if NET5_0_OR_GREATER
        public static bool IsInitOnly(this PropertyInfo property)
        {
            const string ExternalInitModifierName = "IsExternalInit";

            if (!property.CanWrite)
            {
                return false;
            }

            var setMethodReturnParameterModifiers = property
                .SetMethod
                .ReturnParameter
                .GetRequiredCustomModifiers();

            return setMethodReturnParameterModifiers
                .Any(m => m.Name == ExternalInitModifierName);
        }
#endif

    }
}
