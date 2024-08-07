﻿using System;

namespace FluentAssertions.Properties.Common
{
    /// <summary>
    /// A partial copy of FluentAssertions.Common.Guard class (the original class is internal to FluentAssertions)
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

        public static void ThrowIfArgumentIsOutOfRange<T>(T value, string paramName)
            where T : Enum
        {
            if (!Enum.IsDefined(typeof(T), value))
            {
                throw new ArgumentOutOfRangeException(paramName);
            }
        }

        /// <summary>
        /// Workaround to make dotnet_code_quality.null_check_validation_methods work
        /// https://github.com/dotnet/roslyn-analyzers/issues/3451#issuecomment-606690452
        /// </summary>
        [AttributeUsage(AttributeTargets.Parameter)]
        private sealed class ValidatedNotNullAttribute : Attribute
        {
        }
    }
}
