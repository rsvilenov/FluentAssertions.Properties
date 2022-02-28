using System;

namespace FluentAssertions.Properties.Tests.PublicApiTests.TestObjects
{
    public class TestException : Exception
    {
        public TestException() { }
        public TestException(string message = null, Exception innerException = null) : base(message, innerException) { }
    }
}
