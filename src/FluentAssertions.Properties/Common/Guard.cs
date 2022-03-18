using System;

namespace FluentAssertions.Properties.Common
{
    /// <summary>
    /// A partial copy of FluentAssertions.Common.Guard class (he original class is internal to FluentAssertions)
    /// </summary>
    internal static class Guard
    {
        public static void ThrowIfArgumentIsNull<T>(T obj, string paramName)
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
    }
}
