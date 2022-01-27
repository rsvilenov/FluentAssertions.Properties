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
        public void When_selected_properties_throw_from_getter_ThrowFromGetter_InnerException_WithMessage_assert_should_succeed()
        {
            // Arrange
            string exceptionMessage = Guid.NewGuid().ToString();
            var testPropertyMock = new Mock<ITestProperties>();
            testPropertyMock.Setup(o => o.StringProperty).Throws(new TestException(exceptionMessage, new TestException($"Inner {exceptionMessage}")));

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
        public void When_selected_properties_throw_from_getter_with_bad_inner_exception_ThrowFromGetter_InnerException_WithMessage_assert_should_succeed()
        {
            // Arrange
            string exceptionMessage = Guid.NewGuid().ToString();
            var exception = new TestException(exceptionMessage, new InvalidCastException($"Inner {exceptionMessage}"));
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
    }
}
