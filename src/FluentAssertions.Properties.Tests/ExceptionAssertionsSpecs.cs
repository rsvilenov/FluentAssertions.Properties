using FluentAssertions.Properties.Tests.TestObjects;
using Moq;
using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Properties.Tests
{
    public class ExceptionAssertionsSpecs
    {
        [Fact]
        public void When_selected_properties_throw_from_getter_ThrowFromGetter_WithMessage_assert_should_succeed()
        {
            // Arrange
            string exceptionMessage = Guid.NewGuid().ToString();
            var testPropertyMock = new Mock<ITestProperties>();
            testPropertyMock.Setup(o => o.StringProperty).Throws(new TestException(exceptionMessage));

            var valueSourceMock = new Mock<ITestProperties>();
            valueSourceMock.Setup(o => o.StringProperty).Returns(Guid.NewGuid().ToString());

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
            testPropertyMock.Setup(o => o.StringProperty).Throws(new TestException(exceptionMessage));

            var valueSourceMock = new Mock<ITestProperties>();
            valueSourceMock.Setup(o => o.StringProperty).Returns(Guid.NewGuid().ToString());

            var symmetricProperties = testPropertyMock
                .Object
                .Properties(p => p.StringProperty);

            // Act & Assert
            Action assertion = () => symmetricProperties
                .WhenCalledWithValuesFrom(valueSourceMock.Object)
                .Should()
                .ThrowFromGetter<TestException>()
                .WithMessage("expected");

            assertion
                .Should()
                .Throw<XunitException>()
                .WithMessage($"Expected the {nameof(TestException)} message for property {nameof(ITestProperties.StringProperty)} to match the equivalent of*\"expected\", but*\"{exceptionMessage}\" does not.");
        }

        [Fact]
        public void When_selected_properties_throw_from_getter_ThrowFromGetter_InnerException_assert_should_succeed()
        {
            // Arrange
            var testPropertyMock = new Mock<ITestProperties>();
            testPropertyMock.Setup(o => o.StringProperty).Throws(new TestException(string.Empty, new TestException()));

            var valueSourceMock = new Mock<ITestProperties>();
            valueSourceMock.Setup(o => o.StringProperty).Returns(Guid.NewGuid().ToString());

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
            testPropertyMock.Setup(o => o.StringProperty).Throws(exception);

            var valueSourceMock = new Mock<ITestProperties>();
            valueSourceMock.Setup(o => o.StringProperty).Returns(Guid.NewGuid().ToString());

            var symmetricProperties = testPropertyMock
                .Object
                .Properties(p => p.StringProperty);

            // Act & Assert
            Action assertion = () => symmetricProperties
                .WhenCalledWithValuesFrom(valueSourceMock.Object)
                .Should()
                .ThrowFromGetter<TestException>()
                .WithInnerException<TestException>();

            assertion
                .Should()
                .Throw<XunitException>()
                .WithMessage($"Expected inner \"{nameof(TestException)}->{nameof(TestException)}\" for the getter of property \"{nameof(ITestProperties.StringProperty)}\", but found*.");
        }

        [Fact]
        public void When_selected_properties_throw_from_getter_ThrowFromGetter_InnerException_InnerException_assert_should_succeed()
        {
            // Arrange
            var testPropertyMock = new Mock<ITestProperties>();
            testPropertyMock.Setup(o => o.StringProperty).Throws(new TestException(string.Empty, new TestException(string.Empty, new TestException())));

            var valueSourceMock = new Mock<ITestProperties>();
            valueSourceMock.Setup(o => o.StringProperty).Returns(Guid.NewGuid().ToString());

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
            testPropertyMock.Setup(o => o.StringProperty).Throws(new TestException(string.Empty, new TestException(string.Empty, new System.IO.IOException())));

            var valueSourceMock = new Mock<ITestProperties>();
            valueSourceMock.Setup(o => o.StringProperty).Returns(Guid.NewGuid().ToString());

            var symmetricProperties = testPropertyMock
                .Object
                .Properties(p => p.StringProperty);

            // Act & Assert
            Action assertion = () => symmetricProperties
                .WhenCalledWithValuesFrom(valueSourceMock.Object)
                .Should()
                .ThrowFromGetter<TestException>()
                .WithInnerException<TestException>()
                .WithInnerException<TestException>();

            assertion
                .Should()
                .Throw<XunitException>()
                .WithMessage($"Expected inner \"{nameof(TestException)}->{nameof(TestException)}->{nameof(TestException)}\" for the getter of property \"{nameof(ITestProperties.StringProperty)}\", but found \"{nameof(TestException)}->{nameof(TestException)}->{nameof(System.IO.IOException)}\".");
        }


        [Fact]
        public void When_selected_properties_throw_from_getter_ThrowFromGetter_InnerExceptionExactly_assert_should_succeed()
        {
            // Arrange
            var testPropertyMock = new Mock<ITestProperties>();
            testPropertyMock.Setup(o => o.StringProperty).Throws(new TestException(string.Empty, new TestException()));

            var valueSourceMock = new Mock<ITestProperties>();
            valueSourceMock.Setup(o => o.StringProperty).Returns(Guid.NewGuid().ToString());

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
            testPropertyMock.Setup(o => o.StringProperty).Throws(new TestException(string.Empty, new TestException()));

            var valueSourceMock = new Mock<ITestProperties>();
            valueSourceMock.Setup(o => o.StringProperty).Returns(Guid.NewGuid().ToString());

            var symmetricProperties = testPropertyMock
                .Object
                .Properties(p => p.StringProperty);

            // Act & Assert
            Action assertion = () => symmetricProperties
                .WhenCalledWithValuesFrom(valueSourceMock.Object)
                .Should()
                .ThrowFromGetter<TestException>()
                .WithInnerExceptionExactly<Exception>();

            assertion
                .Should()
                .Throw<XunitException>()
                .WithMessage($"Expected inner \"{nameof(TestException)}->{nameof(Exception)}\" for the getter of property \"{nameof(ITestProperties.StringProperty)}\", but found*.");
        }

        [Fact]
        public void When_selected_properties_throw_from_getter_an_exception_without_inner_exception_ThrowFromGetter_InnerException_assert_should_fail()
        {
            // Arrange
            var testPropertyMock = new Mock<ITestProperties>();
            testPropertyMock.Setup(o => o.StringProperty).Throws(new TestException());

            var valueSourceMock = new Mock<ITestProperties>();
            valueSourceMock.Setup(o => o.StringProperty).Returns(Guid.NewGuid().ToString());

            var symmetricProperties = testPropertyMock
                .Object
                .Properties(p => p.StringProperty);

            // Act & Assert
            Action assertion = () => symmetricProperties
                .WhenCalledWithValuesFrom(valueSourceMock.Object)
                .Should()
                .ThrowFromGetter<TestException>()
                .WithInnerException<Exception>();

            assertion
                .Should()
                .Throw<XunitException>()
                .WithMessage($"Expected inner \"{nameof(TestException)}->{nameof(Exception)}\" for the getter of property \"{nameof(ITestProperties.StringProperty)}\", but the thrown exception has no inner exception.");
        }

        [Fact]
        public void When_selected_properties_throw_from_getter_an_exception_without_inner_exception_ThrowFromGetter_InnerExceptionExactly_assert_should_fail()
        {
            // Arrange
            var testPropertyMock = new Mock<ITestProperties>();
            testPropertyMock.Setup(o => o.StringProperty).Throws(new TestException());

            var valueSourceMock = new Mock<ITestProperties>();
            valueSourceMock.Setup(o => o.StringProperty).Returns(Guid.NewGuid().ToString());

            var symmetricProperties = testPropertyMock
                .Object
                .Properties(p => p.StringProperty);

            // Act & Assert
            Action assertion = () => symmetricProperties
                .WhenCalledWithValuesFrom(valueSourceMock.Object)
                .Should()
                .ThrowFromGetter<TestException>()
                .WithInnerExceptionExactly<Exception>();

            assertion
                .Should()
                .Throw<XunitException>()
                .WithMessage($"Expected inner \"{nameof(TestException)}->{nameof(Exception)}\" for the getter of property \"{nameof(ITestProperties.StringProperty)}\", but the thrown exception has no inner exception.");
        }

        [Fact]
        public void When_selected_properties_throw_from_getter_ThrowFromGetter_Where_assert_for_the_exception_message_should_succeed()
        {
            // Arrange
            string exceptionMessage = Guid.NewGuid().ToString();
            var testPropertyMock = new Mock<ITestProperties>();
            testPropertyMock.Setup(o => o.StringProperty).Throws(new TestException(exceptionMessage));

            var valueSourceMock = new Mock<ITestProperties>();
            valueSourceMock.Setup(o => o.StringProperty).Returns(Guid.NewGuid().ToString());

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
        public void When_selected_properties_throw_from_getter_ThrowFromGetter_Where_assert_for_a_wrong_exception_message_should_fail()
        {
            // Arrange
            string exceptionMessage = Guid.NewGuid().ToString();
            var testPropertyMock = new Mock<ITestProperties>();
            testPropertyMock.Setup(o => o.StringProperty).Throws(new TestException(exceptionMessage));

            var valueSourceMock = new Mock<ITestProperties>();
            valueSourceMock.Setup(o => o.StringProperty).Returns(Guid.NewGuid().ToString());

            var symmetricProperties = testPropertyMock
                .Object
                .Properties(p => p.StringProperty);

            // Act & Assert
            Action assertion = () => symmetricProperties
                .WhenCalledWithValuesFrom(valueSourceMock.Object)
                .Should()
                .ThrowFromGetter<TestException>()
                .Where(ex => ex.Message == "some text");

            assertion
                .Should()
                .Throw<XunitException>()
                .WithMessage($"Expected an exception \"{nameof(TestException)}\" for the getter of property \"{nameof(ITestProperties.StringProperty)}\" where *, but the condition was not met by:*");
        }

        [Fact]
        public void When_selected_properties_throw_from_getter_with_inner_exception_ThrowFromGetter_Where_assert_for_the_expected_exception_message_should_succeed()
        {
            // Arrange
            string exceptionMessage = Guid.NewGuid().ToString();
            var testPropertyMock = new Mock<ITestProperties>();
            testPropertyMock.Setup(o => o.StringProperty).Throws(new TestException(string.Empty, new TestException(exceptionMessage)));

            var valueSourceMock = new Mock<ITestProperties>();
            valueSourceMock.Setup(o => o.StringProperty).Returns(Guid.NewGuid().ToString());

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
            testPropertyMock.Setup(o => o.StringProperty).Throws(new TestException(string.Empty, new TestException(exceptionMessage)));

            var valueSourceMock = new Mock<ITestProperties>();
            valueSourceMock.Setup(o => o.StringProperty).Returns(Guid.NewGuid().ToString());

            var symmetricProperties = testPropertyMock
                .Object
                .Properties(p => p.StringProperty);

            // Act & Assert
            Action assertion = () => symmetricProperties
                .WhenCalledWithValuesFrom(valueSourceMock.Object)
                .Should()
                .ThrowFromGetter<TestException>()
                .WithInnerException<TestException>()
                .Where(ex => ex.Message == "some text");

            assertion
                .Should()
                .Throw<XunitException>()
                .WithMessage($"Expected an exception \"{nameof(TestException)}->{nameof(TestException)}\" for the getter of property \"{nameof(ITestProperties.StringProperty)}\" where *, but the condition was not met by:*");
        }
    }
}
