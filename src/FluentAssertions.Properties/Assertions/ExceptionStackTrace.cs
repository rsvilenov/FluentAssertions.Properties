using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;

namespace FluentAssertions.Properties.Assertions
{
    internal static class ExceptionStackTrace
    {
        private static readonly List<string> _testProviderExceptions = new List<string>
        {
            "Xunit.Sdk.XunitException",
            "NUnit.Framework.AssertionException",
            "NSpec.Domain.AssertionException",
            "Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException",
            "Machine.Specifications.SpecificationException"
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static T StartFromCurrentFrame<T>(Func<T> act)
        {
            try
            {
                return act();
            }
            catch (Exception ex)
            {
                if (_testProviderExceptions.Contains(ex.GetType().FullName))
                {
#if DEBUG
                    throw;
#else
#pragma warning disable CA2200
                    throw ex;
#pragma warning restore CA2200
#endif
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
