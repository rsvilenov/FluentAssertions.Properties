using System;
using System.Diagnostics.CodeAnalysis;

namespace FluentAssertions.Properties.Common
{
    /// <summary>
    /// A copy of parts of FluentAssertions.Common.Guard class (he original class is internal to FluentAssertions)
    /// </summary>
    internal static class Guard
    {
        public static void ThrowIfArgumentIsNull<T>([ValidatedNotNull] T obj, string paramName)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        /// <summary>
        /// Workaround to make dotnet_code_quality.null_check_validation_methods work
        /// https://github.com/dotnet/roslyn-analyzers/issues/3451#issuecomment-606690452
        /// </summary>
        [AttributeUsage(AttributeTargets.Parameter)]
        [ExcludeFromCodeCoverage]
        private sealed class ValidatedNotNullAttribute : Attribute
        {
        }
    }
}
