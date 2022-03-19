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
            public void When_selected_properties_throw_from_setter_the_stack_trace_in_the_error_message_should_be_correct_and_not_include_frames_from_this_library()
            {
               var testAction = () =>
                    new StackTraceTests()
                        .Properties()
                        .ExactlyOfType<int>()
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
