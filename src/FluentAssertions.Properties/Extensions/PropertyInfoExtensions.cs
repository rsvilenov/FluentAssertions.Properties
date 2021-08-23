using System.Reflection;

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
    }
}
