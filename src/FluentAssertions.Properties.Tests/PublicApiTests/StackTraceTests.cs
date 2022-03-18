using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Properties.Tests.PublicApiTests
{
    public partial class InvocationAssertionsTests
    {
        public class StackTraceTests
        {
#pragma warning disable xUnit1013
            public void Throw()
            {
                throw new NullReferenceException();
            }
#pragma warning restore xUnit1013

            public int MyProperty 
            { 
                get 
                { 
                    return 2; 
                } 
                set 
                { 
                    Throw(); 
                } 
            }


            [Fact]
            public void Test1()
            {
               var testAction = () =>
                    new StackTraceTests()
                        .Properties()
                        .OfType<int>()
                        .WhenCalledWith(1)
                        .Should()
                        .ThrowFromSetter<ArgumentNullException>();

                testAction
                    .Should()
                    .Throw<XunitException>()
                    .WithMessage($"*{nameof(StackTraceTests)}.{nameof(Throw)}*")
                    .And.Message.Should().NotContain("FluentAssertions.Properties.Assertions");
            }
        }
    }
}
