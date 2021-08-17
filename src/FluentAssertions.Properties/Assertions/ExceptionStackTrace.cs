using System;
using System.Runtime.CompilerServices;

namespace FluentAssertions.Properties.Assertions
{
    internal static class ExceptionStackTrace
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static T StartFromCurrentFrame<T>(Func<T> act)
        {
            try
            {
                return act();
            }
            catch (Exception ex)
            {
#if DEBUG
                throw;
#else
                throw ex;
#endif
            }
        }
    }
}
