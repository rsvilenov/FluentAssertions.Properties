﻿using Moq;
using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Properties.Tests
{
    public class InvocationAssertionsSpecs
    {
        [Fact]
        public void When_selected_symmetric_properties_are_called_with_values_from_source_object_ProvideSymmetricAccess_assert_should_succeed()
        {
            // Arrange
            var testObj = Mock.Of<ITestProperties>();
            var valueSourceMock = new Mock<ITestProperties>();
            valueSourceMock.Setup(o => o.StringProperty).Returns(Guid.NewGuid().ToString());
            valueSourceMock.Setup(o => o.IntProperty).Returns(new Random().Next());
            var symmetricProperties = testObj
                .Properties(p => p.StringProperty, p => p.IntProperty);

            // Act & Assert
            symmetricProperties
                .WhenCalledWithValuesFrom(valueSourceMock.Object)
                .Should()
                .ProvideSymmetricAccess();
        }

        [Fact]
        public void When_selected_assymetric_properties_are_called_with_values_from_source_object_ProvideSymmetricAccess_assert_should_fail()
        {
            // Arrange
            var testPropertyMock = new Mock<ITestProperties>();
            testPropertyMock.Setup(o => o.StringProperty).Returns(Guid.NewGuid().ToString());
            testPropertyMock.Setup(o => o.IntProperty).Returns(new Random().Next());

            var valueSourceMock = new Mock<ITestProperties>();
            valueSourceMock.Setup(o => o.StringProperty).Returns(Guid.NewGuid().ToString());
            valueSourceMock.Setup(o => o.IntProperty).Returns(new Random().Next());

            var symmetricProperties = testPropertyMock
                .Object
                .Properties(p => p.StringProperty, p => p.IntProperty);

            // Act & Assert
            Action action = () =>
                symmetricProperties
                    .WhenCalledWithValuesFrom(valueSourceMock.Object)
                    .Should()
                    .ProvideSymmetricAccess();

            action
                .Should()
                .Throw<XunitException>()
                .WithMessage("Expected the get and set operations of property *");
        }

        [Fact]
        public void When_selected_getter_only_properties_are_called_with_values_from_source_object_ProvideSymmetricAccess_assert_should_fail()
        {
            // Arrange
            var testPropertyMock = new Mock<ITestProperties>();

            var valueSourceMock = new Mock<ITestProperties>();
            valueSourceMock.Setup(o => o.ReadOnlyStringProperty).Returns(Guid.NewGuid().ToString());

            var symmetricProperties = testPropertyMock
                .Object
                .Properties(p => p.ReadOnlyStringProperty);

            // Act & Assert
            Action action = () =>
                symmetricProperties
                    .WhenCalledWithValuesFrom(valueSourceMock.Object)
                    .Should()
                    .ProvideSymmetricAccess();

            action
                .Should()
                .Throw<XunitException>()
                .WithMessage("Expected property * to be writable, but was not.");
        }

        [Fact]
        public void When_selected_properties_throw_from_getter_ProvideSymmetricAccess_assert_should_fail()
        {
            // Arrange
            var testPropertyMock = new Mock<ITestProperties>();
            testPropertyMock.Setup(o => o.StringProperty).Throws<TestException>();

            var valueSourceMock = new Mock<ITestProperties>();
            valueSourceMock.Setup(o => o.ReadOnlyStringProperty).Returns(Guid.NewGuid().ToString());

            var symmetricProperties = testPropertyMock
                .Object
                .Properties(p => p.StringProperty);

            // Act & Assert
            Action action = () =>
                symmetricProperties
                    .WhenCalledWithValuesFrom(valueSourceMock.Object)
                    .Should()
                    .ProvideSymmetricAccess();

            action
                .Should()
                .Throw<XunitException>()
                .WithMessage("Did not expect any exceptions for property *, but got *");
        }

        [Fact]
        public void When_selected_properties_throw_from_getter_ThrowFromGetter_assert_should_succeed()
        {
            // Arrange
            var testPropertyMock = new Mock<ITestProperties>();
            testPropertyMock.Setup(o => o.StringProperty).Throws<TestException>();

            var valueSourceMock = new Mock<ITestProperties>();
            valueSourceMock.Setup(o => o.StringProperty).Returns(Guid.NewGuid().ToString());

            var symmetricProperties = testPropertyMock
                .Object
                .Properties(p => p.StringProperty);

            // Act & Assert
            symmetricProperties
                .WhenCalledWithValuesFrom(valueSourceMock.Object)
                .Should()
                .ThrowFromGetter<TestException>();
        }

        [Fact]
        public void When_selected_properties_throw_from_setter_ThrowFromGetter_assert_should_fail()
        {
            // Arrange
            var testPropertyMock = new Mock<ITestProperties>();
            testPropertyMock
                .SetupSet(o => o.StringProperty = string.Empty)
                .Throws<TestException>();

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
                .ThrowFromGetter<TestException>();

            assertion
                .Should()
                .Throw<XunitException>()
                .WithMessage("Expected property \"getter\" of property * to throw *");
        }

        [Fact]
        public void When_selected_properties_throw_from_setter_ThrowFromSetter_assert_should_succeed()
        {
            // Arrange
            var testPropertyMock = new Mock<ITestProperties>();
            string guid = Guid.NewGuid().ToString();
            testPropertyMock
                .SetupSet(o => o.StringProperty = guid)
                .Throws<TestException>();

            var valueSourceMock = new Mock<ITestProperties>();
            valueSourceMock
                .Setup(o => o.StringProperty)
                .Returns(guid);

            var symmetricProperties = testPropertyMock
                .Object
                .Properties(p => p.StringProperty);

            // Act & Assert
            symmetricProperties
                .WhenCalledWithValuesFrom(valueSourceMock.Object)
                .Should()
                .ThrowFromSetter<TestException>();
        }

        [Fact]
        public void When_selected_properties_throw_from_getter_a_derived_exception_type_ThrowFromGetterExactly_assert_should_fail()
        {
            // Arrange
            var testPropertyMock = new Mock<ITestProperties>();
            testPropertyMock.Setup(o => o.StringProperty).Throws<TestException>();

            var valueSourceMock = new Mock<ITestProperties>();
            valueSourceMock.Setup(o => o.StringProperty).Returns(Guid.NewGuid().ToString());

            var symmetricProperties = testPropertyMock
                .Object
                .Properties(p => p.StringProperty);

            // Act & Assert
            Action assertion = () => symmetricProperties
                .WhenCalledWithValuesFrom(valueSourceMock.Object)
                .Should()
                .ThrowFromGetterExactly<Exception>();

            assertion
                .Should()
                .Throw<XunitException>()
                .WithMessage("Expected property \"getter\" of property * to throw *");
        }

        [Fact]
        public void When_selected_properties_throw_from_setter_a_derived_exception_type_ThrowFromSetterExactly_assert_should_fail()
        {
            // Arrange
            var testPropertyMock = new Mock<ITestProperties>();
            string guid = Guid.NewGuid().ToString();
            testPropertyMock
                .SetupSet(o => o.StringProperty = guid)
                .Throws<TestException>();

            var valueSourceMock = new Mock<ITestProperties>();
            valueSourceMock
                .Setup(o => o.StringProperty)
                .Returns(guid);

            var symmetricProperties = testPropertyMock
                .Object
                .Properties(p => p.StringProperty);

            // Act & Assert
            Action assertion = () => symmetricProperties
                .WhenCalledWithValuesFrom(valueSourceMock.Object)
                .Should()
                .ThrowFromSetterExactly<Exception>();

            assertion
                .Should()
                .Throw<XunitException>()
                .WithMessage("Expected property \"setter\" of property * to throw *");
        }

        public interface ITestProperties
        {
            string StringProperty { get; set; }
            int IntProperty { get; set; }
            string ReadOnlyStringProperty { get; }
            string WriteOnlyStringProperty { set; }
        }

        public class TestException : Exception
        { 
            public TestException() { }
            public TestException(string message = null, Exception innerException = null) : base(message, innerException) { }
        }
    }
}
