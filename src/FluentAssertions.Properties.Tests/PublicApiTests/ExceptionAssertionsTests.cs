using FluentAssertions.Properties.Tests.PublicApiTests.TestObjects;
using Moq;
using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Properties.Tests.PublicApiTests
{
    public class ExceptionAssertionsTests : PublicApiTestBase
    {
        [Fact]
        public void When_selected_properties_are_called_with_WhenCalledWithValuesFrom_with_null_parameter_it_should_throw_ArgumentNullException()
        {
            // Arrange
            var testPropertyMock = new Mock<ITestProperties>();
            ITestProperties valueSource = null;
            var symmetricProperties = testPropertyMock
                .Object
                .Properties(p => p.StringProperty);
            Action testAction = () => symmetricProperties
                .WhenCalledWithValuesFrom(valueSource);

            // Act & Assert
            testAction
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void When_selected_properties_throw_from_getter_ThrowFromGetter_WithMessage_assert_should_succeed()
        {
            // Arrange
            string exceptionMessage = Guid.NewGuid().ToString();
            var testPropertyMock = new Mock<ITestProperties>();
            testPropertyMock
                .Setup(o => o.StringProperty)
                .Throws(new TestException(exceptionMessage));

            var valueSourceMock = new Mock<ITestProperties>();
            valueSourceMock
                .Setup(o => o.StringProperty)
                .Returns(Guid.NewGuid().ToString());

            var symmetricProperties = testPropertyMock
                .Object
                .Properties(p => p.StringProperty);

            // Act & Assert
            symmetricProperties
                .WhenCalledWithValuesFrom(valueSourceMock.Object)
                .Should()
                .ThrowFromGetter<TestException>()
                .WithMessage(exceptionMessage);
        }

        [Fact]
        public void When_selected_properties_throw_from_getter_with_bad_exception_message_ThrowFromGetter_WithMessage_assert_should_fail()
        {
            // Arrange
            string exceptionMessage = Guid.NewGuid().ToString();
            var testPropertyMock = new Mock<ITestProperties>();
            testPropertyMock
                .Setup(o => o.StringProperty)
                .Throws(new TestException(exceptionMessage));

            var valueSourceMock = new Mock<ITestProperties>();
            valueSourceMock
                .Setup(o => o.StringProperty)
                .Returns(Guid.NewGuid().ToString());

            var symmetricProperties = testPropertyMock
                .Object
                .Properties(p => p.StringProperty);

            var assertReason = base.CreateAssertReason();

            // Act & Assert
            Action assertion = () => symmetricProperties
                .WhenCalledWithValuesFrom(valueSourceMock.Object)
                .Should()
                .ThrowFromGetter<TestException>()
                .WithMessage("expected", assertReason.BecauseWithFormat, assertReason.BecauseArg1, assertReason.BecauseArg2);

            assertion
                .Should()
                .Throw<XunitException>()
                .WithMessage($"Expected the {nameof(TestException)} message for property {nameof(ITestProperties.StringProperty)} to match the equivalent of*\"expected\" because {assertReason}, but*\"{exceptionMessage}\" does not.");
        }

        [Fact]
        public void When_selected_properties_throw_from_getter_ThrowFromGetter_InnerException_assert_should_succeed()
        {
            // Arrange
            var testPropertyMock = new Mock<ITestProperties>();
            testPropertyMock
                .Setup(o => o.StringProperty)
                .Throws(new TestException(string.Empty, new TestException()));

            var valueSourceMock = new Mock<ITestProperties>();
            valueSourceMock
                .Setup(o => o.StringProperty)
                .Returns(Guid.NewGuid().ToString());

            var symmetricProperties = testPropertyMock
                .Object
                .Properties(p => p.StringProperty);

            // Act & Assert
            symmetricProperties
                .WhenCalledWithValuesFrom(valueSourceMock.Object)
                .Should()
                .ThrowFromGetter<TestException>()
                .WithInnerException<TestException>();
        }

        [Fact]
        public void When_selected_properties_throw_from_getter_with_bad_inner_exception_ThrowFromGetter_InnerException_assert_should_fail()
        {
            // Arrange
            var exception = new TestException(string.Empty, new InvalidCastException());
            var testPropertyMock = new Mock<ITestProperties>();
            testPropertyMock
                .Setup(o => o.StringProperty)
                .Throws(exception);

            var valueSourceMock = new Mock<ITestProperties>();
            valueSourceMock
                .Setup(o => o.StringProperty)
                .Returns(Guid.NewGuid().ToString());

            var symmetricProperties = testPropertyMock
                .Object
                .Properties(p => p.StringProperty);

            var assertReason = base.CreateAssertReason();

            // Act & Assert
            Action assertion = () => symmetricProperties
                .WhenCalledWithValuesFrom(valueSourceMock.Object)
                .Should()
                .ThrowFromGetter<TestException>()
                .WithInnerException<TestException>(assertReason.BecauseWithFormat, assertReason.BecauseArg1, assertReason.BecauseArg2);

            assertion
                .Should()
                .Throw<XunitException>()
                .WithMessage($"Expected inner \"{nameof(TestException)}->{nameof(TestException)}\" because {assertReason} for the getter of property \"{nameof(ITestProperties.StringProperty)}\", but found*.");
        }

        [Fact]
        public void When_selected_properties_throw_from_getter_ThrowFromGetter_InnerException_InnerException_assert_should_succeed()
        {
            // Arrange
            var testPropertyMock = new Mock<ITestProperties>();
            testPropertyMock
                .Setup(o => o.StringProperty)
                .Throws(new TestException(string.Empty, new TestException(string.Empty, new TestException())));

            var valueSourceMock = new Mock<ITestProperties>();
            valueSourceMock
                .Setup(o => o.StringProperty)
                .Returns(Guid.NewGuid().ToString());

            var symmetricProperties = testPropertyMock
                .Object
                .Properties(p => p.StringProperty);

            // Act & Assert
            symmetricProperties
                .WhenCalledWithValuesFrom(valueSourceMock.Object)
                .Should()
                .ThrowFromGetter<TestException>()
                .WithInnerException<TestException>()
                .WithInnerException<TestException>();
        }

        [Fact]
        public void When_selected_properties_throw_from_getter_with_bad_innermost_exception_ThrowFromGetter_InnerException_InnerException_assert_should_fail()
        {
            // Arrange
            var testPropertyMock = new Mock<ITestProperties>();
            testPropertyMock
                .Setup(o => o.StringProperty)
                .Throws(new TestException(string.Empty, new TestException(string.Empty, new System.IO.IOException())));

            var valueSourceMock = new Mock<ITestProperties>();
            valueSourceMock
                .Setup(o => o.StringProperty)
                .Returns(Guid.NewGuid().ToString());

            var symmetricProperties = testPropertyMock
                .Object
                .Properties(p => p.StringProperty);

            var assertReason = base.CreateAssertReason();

            // Act & Assert
            Action assertion = () => symmetricProperties
                .WhenCalledWithValuesFrom(valueSourceMock.Object)
                .Should()
                .ThrowFromGetter<TestException>()
                .WithInnerException<TestException>()
                .WithInnerException<TestException>(assertReason.BecauseWithFormat, assertReason.BecauseArg1, assertReason.BecauseArg2);

            assertion
                .Should()
                .Throw<XunitException>()
                .WithMessage($"Expected inner \"{nameof(TestException)}->{nameof(TestException)}->{nameof(TestException)}\" because {assertReason} for the getter of property \"{nameof(ITestProperties.StringProperty)}\", but found \"{nameof(TestException)}->{nameof(TestException)}->{nameof(System.IO.IOException)}\".");
        }


        [Fact]
        public void When_selected_properties_throw_from_getter_ThrowFromGetter_InnerExceptionExactly_assert_should_succeed()
        {
            // Arrange
            var testPropertyMock = new Mock<ITestProperties>();
            testPropertyMock
                .Setup(o => o.StringProperty)
                .Throws(new TestException(string.Empty, new TestException()));

            var valueSourceMock = new Mock<ITestProperties>();
            valueSourceMock
                .Setup(o => o.StringProperty)
                .Returns(Guid.NewGuid().ToString());

            var symmetricProperties = testPropertyMock
                .Object
                .Properties(p => p.StringProperty);

            // Act & Assert
            symmetricProperties
                .WhenCalledWithValuesFrom(valueSourceMock.Object)
                .Should()
                .ThrowFromGetter<TestException>()
                .WithInnerExceptionExactly<TestException>();
        }

        [Fact]
        public void When_selected_properties_throw_from_getter_ThrowFromGetter_InnerExceptionExactly_assert_for_base_exception_class_should_fail()
        {
            // Arrange
            var testPropertyMock = new Mock<ITestProperties>();
            testPropertyMock
                .Setup(o => o.StringProperty)
                .Throws(new TestException(string.Empty, new TestException()));

            var valueSourceMock = new Mock<ITestProperties>();
            valueSourceMock
                .Setup(o => o.StringProperty)
                .Returns(Guid.NewGuid().ToString());

            var symmetricProperties = testPropertyMock
                .Object
                .Properties(p => p.StringProperty);

            var assertReason = base.CreateAssertReason();

            // Act & Assert
            Action assertion = () => symmetricProperties
                .WhenCalledWithValuesFrom(valueSourceMock.Object)
                .Should()
                .ThrowFromGetter<TestException>()
                .WithInnerExceptionExactly<Exception>(assertReason.BecauseWithFormat, assertReason.BecauseArg1, assertReason.BecauseArg2);

            assertion
                .Should()
                .Throw<XunitException>()
                .WithMessage($"Expected inner \"{nameof(TestException)}->{nameof(Exception)}\" because {assertReason} for the getter of property \"{nameof(ITestProperties.StringProperty)}\", but found*.");
        }

        [Fact]
        public void When_selected_properties_throw_from_getter_an_exception_without_inner_exception_ThrowFromGetter_InnerException_assert_should_fail()
        {
            // Arrange
            var testPropertyMock = new Mock<ITestProperties>();
            testPropertyMock
                .Setup(o => o.StringProperty)
                .Throws(new TestException());

            var valueSourceMock = new Mock<ITestProperties>();
            valueSourceMock
                .Setup(o => o.StringProperty)
                .Returns(Guid.NewGuid().ToString());

            var symmetricProperties = testPropertyMock
                .Object
                .Properties(p => p.StringProperty);

            var assertReason = base.CreateAssertReason();

            // Act & Assert
            Action assertion = () => symmetricProperties
                .WhenCalledWithValuesFrom(valueSourceMock.Object)
                .Should()
                .ThrowFromGetter<TestException>()
                .WithInnerException<Exception>(assertReason.BecauseWithFormat, assertReason.BecauseArg1, assertReason.BecauseArg2);

            assertion
                .Should()
                .Throw<XunitException>()
                .WithMessage($"Expected inner \"{nameof(TestException)}->{nameof(Exception)}\" because {assertReason} for the getter of property \"{nameof(ITestProperties.StringProperty)}\", but the thrown exception has no inner exception.");
        }

        [Fact]
        public void When_selected_properties_throw_from_getter_an_exception_with_inner_exception_with_no_inner_exception_ThrowFromGetter_InnerException_InnerException_assert_should_fail()
        {
            // Arrange
            var testPropertyMock = new Mock<ITestProperties>();
            testPropertyMock
                .Setup(o => o.StringProperty)
                .Throws(new TestException(string.Empty, new InvalidCastException()));

            var valueSourceMock = new Mock<ITestProperties>();
            valueSourceMock
                .Setup(o => o.StringProperty)
                .Returns(Guid.NewGuid().ToString());

            var symmetricProperties = testPropertyMock
                .Object
                .Properties(p => p.StringProperty);

            var assertReason = base.CreateAssertReason();

            // Act & Assert
            Action assertion = () => symmetricProperties
                .WhenCalledWithValuesFrom(valueSourceMock.Object)
                .Should()
                .ThrowFromGetter<TestException>()
                .WithInnerException<InvalidCastException>()
                .WithInnerException<Exception>(assertReason.BecauseWithFormat, assertReason.BecauseArg1, assertReason.BecauseArg2);

            assertion
                .Should()
                .Throw<XunitException>()
                .WithMessage($"Expected inner \"{nameof(TestException)}->{nameof(InvalidCastException)}->{nameof(Exception)}\" because {assertReason} for the getter of property \"{nameof(ITestProperties.StringProperty)}\", but the inner exception has no inner exception.");
        }

        [Fact]
        public void When_selected_properties_throw_from_getter_an_exception_without_inner_exception_ThrowFromGetter_InnerException_with_reason_param_assert_should_fail_with_expected_reason()
        {
            // Arrange
            var testPropertyMock = new Mock<ITestProperties>();
            testPropertyMock
                .Setup(o => o.StringProperty)
                .Throws(new TestException());

            var valueSourceMock = new Mock<ITestProperties>();
            valueSourceMock
                .Setup(o => o.StringProperty)
                .Returns(Guid.NewGuid().ToString());

            var symmetricProperties = testPropertyMock
                .Object
                .Properties(p => p.StringProperty);

            var assertReason = base.CreateAssertReason();

            // Act & Assert
            Action assertion = () => symmetricProperties
                .WhenCalledWithValuesFrom(valueSourceMock.Object)
                .Should()
                .ThrowFromGetter<TestException>()
                .WithInnerException<Exception>(assertReason.BecauseWithFormat, assertReason.BecauseArg1, assertReason.BecauseArg2);

            assertion
                .Should()
                .Throw<XunitException>()
                .WithMessage($"Expected inner \"{nameof(TestException)}->{nameof(Exception)}\" because {assertReason} for the getter of property \"{nameof(ITestProperties.StringProperty)}\", but the thrown exception has no inner exception.");
        }

        [Fact]
        public void When_selected_properties_throw_from_getter_an_exception_with_unexpected_inner_exception_ThrowFromGetter_InnerException_with_reason_param_assert_should_fail_with_expected_reason()
        {
            // Arrange
            var testPropertyMock = new Mock<ITestProperties>();
            testPropertyMock
                .Setup(o => o.StringProperty)
                .Throws(new TestException(string.Empty, new InvalidCastException()));

            var valueSourceMock = new Mock<ITestProperties>();
            valueSourceMock
                .Setup(o => o.StringProperty)
                .Returns(Guid.NewGuid().ToString());

            var symmetricProperties = testPropertyMock
                .Object
                .Properties(p => p.StringProperty);

            var assertReason = base.CreateAssertReason();

            // Act & Assert
            Action assertion = () => symmetricProperties
                .WhenCalledWithValuesFrom(valueSourceMock.Object)
                .Should()
                .ThrowFromGetter<TestException>()
                .WithInnerException<InvalidOperationException>(assertReason.BecauseWithFormat, assertReason.BecauseArg1, assertReason.BecauseArg2);

            assertion
                .Should()
                .Throw<XunitException>()
                .WithMessage($"Expected inner \"{nameof(TestException)}->{nameof(InvalidOperationException)}\" because {assertReason} for the getter of property \"{nameof(ITestProperties.StringProperty)}\", but found \"{nameof(TestException)}->{nameof(InvalidCastException)}\".");
        }

        [Fact]
        public void When_selected_properties_throw_from_getter_an_exception_without_inner_exception_ThrowFromGetter_InnerExceptionExactly_assert_should_fail()
        {
            // Arrange
            var testPropertyMock = new Mock<ITestProperties>();
            testPropertyMock
                .Setup(o => o.StringProperty)
                .Throws(new TestException());

            var valueSourceMock = new Mock<ITestProperties>();
            valueSourceMock
                .Setup(o => o.StringProperty)
                .Returns(Guid.NewGuid().ToString());

            var symmetricProperties = testPropertyMock
                .Object
                .Properties(p => p.StringProperty);

            var assertReason = base.CreateAssertReason();

            // Act & Assert
            Action assertion = () => symmetricProperties
                .WhenCalledWithValuesFrom(valueSourceMock.Object)
                .Should()
                .ThrowFromGetter<TestException>()
                .WithInnerExceptionExactly<Exception>(assertReason.BecauseWithFormat, assertReason.BecauseArg1, assertReason.BecauseArg2);

            assertion
                .Should()
                .Throw<XunitException>()
                .WithMessage($"Expected inner \"{nameof(TestException)}->{nameof(Exception)}\" because {assertReason} for the getter of property \"{nameof(ITestProperties.StringProperty)}\", but the thrown exception has no inner exception.");
        }

        [Fact]
        public void When_selected_properties_throw_from_getter_ThrowFromGetter_Where_assert_for_the_exception_message_should_succeed()
        {
            // Arrange
            string exceptionMessage = Guid.NewGuid().ToString();
            var testPropertyMock = new Mock<ITestProperties>();
            testPropertyMock
                .Setup(o => o.StringProperty)
                .Throws(new TestException(exceptionMessage));

            var valueSourceMock = new Mock<ITestProperties>();
            valueSourceMock
                .Setup(o => o.StringProperty)
                .Returns(Guid.NewGuid().ToString());

            var symmetricProperties = testPropertyMock
                .Object
                .Properties(p => p.StringProperty);

            // Act & Assert
            symmetricProperties
                .WhenCalledWithValuesFrom(valueSourceMock.Object)
                .Should()
                .ThrowFromGetter<TestException>()
                .Where(ex => ex.Message == exceptionMessage);
        }

        [Fact]
        public void When_selected_properties_throw_from_getter_ThrowFromGetter_Where_assert_with_null_delegate_should_fail()
        {
            // Arrange
            var testPropertyMock = new Mock<ITestProperties>();
            testPropertyMock
                .Setup(o => o.StringProperty)
                .Throws(new TestException());

            var symmetricProperties = testPropertyMock
                .Object
                .Properties(p => p.StringProperty);

            Action assertAction = () => symmetricProperties
                .WhenCalledWith(string.Empty)
                .Should()
                .ThrowFromGetter<TestException>()
                .Where(null);

            // Act & Assert
            assertAction
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void When_selected_properties_throw_from_getter_ThrowFromGetter_Where_assert_for_a_wrong_exception_message_should_fail()
        {
            // Arrange
            string exceptionMessage = Guid.NewGuid().ToString();
            var testPropertyMock = new Mock<ITestProperties>();
            testPropertyMock
                .Setup(o => o.StringProperty)
                .Throws(new TestException(exceptionMessage));

            var valueSourceMock = new Mock<ITestProperties>();
            valueSourceMock
                .Setup(o => o.StringProperty)
                .Returns(Guid.NewGuid().ToString());

            var symmetricProperties = testPropertyMock
                .Object
                .Properties(p => p.StringProperty);

            var assertReason = base.CreateAssertReason();

            // Act & Assert
            Action assertion = () => symmetricProperties
                .WhenCalledWithValuesFrom(valueSourceMock.Object)
                .Should()
                .ThrowFromGetter<TestException>()
                .Where(ex => ex.Message == "some text", assertReason.BecauseWithFormat, assertReason.BecauseArg1, assertReason.BecauseArg2);

            assertion
                .Should()
                .Throw<XunitException>()
                .WithMessage($"Expected an exception \"{nameof(TestException)}\" for the getter of property \"{nameof(ITestProperties.StringProperty)}\" where * because {assertReason}, but the condition was not met by:*");
        }

        [Fact]
        public void When_selected_properties_throw_from_getter_with_inner_exception_ThrowFromGetter_Where_assert_for_the_expected_exception_message_should_succeed()
        {
            // Arrange
            string exceptionMessage = Guid.NewGuid().ToString();
            var testPropertyMock = new Mock<ITestProperties>();
            testPropertyMock
                .Setup(o => o.StringProperty)
                .Throws(new TestException(string.Empty, new TestException(exceptionMessage)));

            var valueSourceMock = new Mock<ITestProperties>();
            valueSourceMock
                .Setup(o => o.StringProperty)
                .Returns(Guid.NewGuid().ToString());

            var symmetricProperties = testPropertyMock
                .Object
                .Properties(p => p.StringProperty);

            // Act & Assert
            Action assertion = () => symmetricProperties
                .WhenCalledWithValuesFrom(valueSourceMock.Object)
                .Should()
                .ThrowFromGetter<TestException>()
                .WithInnerException<TestException>()
                .Where(ex => ex.Message == exceptionMessage);
        }

        [Fact]
        public void When_selected_properties_throw_from_getter_with_inner_exception_ThrowFromGetter_Where_assert_for_a_wrong_exception_message_should_fail()
        {
            // Arrange
            string exceptionMessage = Guid.NewGuid().ToString();
            var testPropertyMock = new Mock<ITestProperties>();
            testPropertyMock
                .Setup(o => o.StringProperty)
                .Throws(new TestException(string.Empty, new TestException(exceptionMessage)));

            var valueSourceMock = new Mock<ITestProperties>();
            valueSourceMock
                .Setup(o => o.StringProperty)
                .Returns(Guid.NewGuid().ToString());

            var symmetricProperties = testPropertyMock
                .Object
                .Properties(p => p.StringProperty);

            var assertReason = base.CreateAssertReason();

            // Act & Assert
            Action assertion = () => symmetricProperties
                .WhenCalledWithValuesFrom(valueSourceMock.Object)
                .Should()
                .ThrowFromGetter<TestException>()
                .WithInnerException<TestException>()
                .Where(ex => ex.Message == "some text", assertReason.BecauseWithFormat, assertReason.BecauseArg1, assertReason.BecauseArg2);

            assertion
                .Should()
                .Throw<XunitException>()
                .WithMessage($"Expected an exception \"{nameof(TestException)}->{nameof(TestException)}\" for the getter of property \"{nameof(ITestProperties.StringProperty)}\" where * because {assertReason}, but the condition was not met by:*");
        }
    }
}
