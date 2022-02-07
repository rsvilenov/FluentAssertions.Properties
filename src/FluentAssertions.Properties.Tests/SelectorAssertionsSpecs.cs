using FluentAssertions.Properties.Selectors;
using FluentAssertions.Properties.Tests.TestObjects;
using System;
using System.Linq;
using Xunit;

namespace FluentAssertions.Properties.Tests
{
    public class SelectorAssertionsSpecs
    {
        [Fact]
        public void When_selecting_all_properties_of_a_class_HaveCount_assert_count_should_match()
        {
            // Arrange
            var testObj = new TestClass();
            var selector =
                testObj.Properties();
            int count = selector.Count();

            // Act & Assert
            selector.Should().HaveCount(count);
        }

        [Fact]
        public void When_selecting_all_properties_of_a_class_HaveCount_assert_with_minus_one_should_fail()
        {
            // Arrange
            const int expectedCount = -1;
            var testObj = new TestClass();
            var selector =
                testObj.Properties();

            // Act & Assert
            Action assertion = () 
                => selector.Should().HaveCount(expectedCount);

            assertion
                .Should()
                .Throw<Xunit.Sdk.XunitException>()
                .WithMessage($"Expected property count of type {typeof(TestClass)} to be {expectedCount}, but was not.");
        }

        [Fact]
        public void When_selecting_all_properties_of_primitive_types_BeOfPrimitiveType_should_succeed()
        {
            // Arrange
            var testObj = new TestClass();
            var selector =
                testObj.Properties().ThatAreOfPrimitiveTypes;

            // Act & Assert
            selector.Should().BeOfPrimitiveType();
        }

        [Fact]
        public void When_selecting_all_properties_of_nonprimitive_types_BeOfPrimitiveType_should_fail()
        {
            // Arrange
            var testObj = new TestClass();
            var selector =
                testObj.Properties().ThatAreNotOfPrimitiveTypes;

            // Act & Assert
            Action assertion = ()
                => selector.Should().BeOfPrimitiveType();

            assertion
                .Should()
                .Throw<Xunit.Sdk.XunitException>()
                .WithMessage("Expected property * to be of primitive type, but was not.");
        }

        [Fact]
        public void When_selecting_all_properties_of_nonprimitive_types_NotBeOfPrimitiveType_should_succeed()
        {
            // Arrange
            var testObj = new TestClass();
            var selector =
                testObj.Properties().ThatAreNotOfPrimitiveTypes;

            // Act & Assert
            selector.Should().NotBeOfPrimitiveType();
        }

        [Fact]
        public void When_selecting_all_properties_of_primitive_types_NotBeOfPrimitiveType_should_fail()
        {
            // Arrange
            var testObj = new TestClass();
            var selector =
                testObj.Properties().ThatAreOfPrimitiveTypes;

            // Act & Assert
            Action assertion = ()
                => selector.Should().NotBeOfPrimitiveType();

            assertion
                .Should()
                .Throw<Xunit.Sdk.XunitException>()
                .WithMessage("Expected property * not to be of primitive type, but was.");
        }

        [Fact]
        public void When_selecting_all_properties_of_value_types_BeOfValueType_should_succeed()
        {
            // Arrange
            var testObj = new TestClass();
            var selector =
                testObj.Properties().ThatAreOfValueType;

            // Act & Assert
            selector.Should().BeOfValueType();
        }

        [Fact]
        public void When_selecting_all_properties_of_reference_types_BeOfValueType_should_fail()
        {
            // Arrange
            var testObj = new TestClass();
            var selector =
                testObj.Properties().ThatAreOfReferenceType;

            // Act & Assert
            Action assertion = ()
                => selector.Should().BeOfValueType();

            assertion
                .Should()
                .Throw<Xunit.Sdk.XunitException>()
                .WithMessage("Expected property * to be of value type, but was not.");
        }

        [Fact]
        public void When_selecting_all_properties_of_reference_types_BeOfReferenceType_should_succeed()
        {
            // Arrange
            var testObj = new TestClass();
            var selector =
                testObj.Properties().ThatAreOfReferenceType;

            // Act & Assert
            selector.Should().BeOfReferenceType();
        }

        [Fact]
        public void When_selecting_all_properties_of_value_types_BeOfReferenceType_should_fail()
        {
            // Arrange
            var testObj = new TestClass();
            var selector =
                testObj.Properties().ThatAreOfValueType;

            // Act & Assert
            Action assertion = ()
                => selector.Should().BeOfReferenceType();

            assertion
                .Should()
                .Throw<Xunit.Sdk.XunitException>()
                .WithMessage("Expected property * to be of reference type, but was not.");
        }

        [Fact]
        public void When_selecting_all_virtual_properties_BeVirtual_should_succeed()
        {
            // Arrange
            var testObj = new TestClass();
            var selector =
                testObj.Properties().ThatAreVirtual;

            // Act & Assert
            selector.Should().BeVirtual();
        }

        [Fact]
        public void When_selecting_all_nonvirtual_properties_BeVirtual_should_fail()
        {
            // Arrange
            var testObj = new TestClass();
            var selector =
                testObj.Properties().ThatAreNotVirtual;

            // Act & Assert
            Action assertion = ()
                => selector.Should().BeVirtual();

            assertion
                .Should()
                .Throw<Xunit.Sdk.XunitException>()
                .WithMessage("Expected property * to be virtual, but it is not.");
        }

        [Fact]
        public void When_selecting_all_nonvirtual_properties_NotBeVirtual_should_succeed()
        {
            // Arrange
            var testObj = new TestClass();
            var selector =
                testObj.Properties().ThatAreNotVirtual;

            // Act & Assert
            selector.Should().NotBeVirtual();
        }

        [Fact]
        public void When_selecting_all_virtual_properties_NotBeVirtual_should_fail()
        {
            // Arrange
            var testObj = new TestClass();
            var selector =
                testObj.Properties().ThatAreVirtual;

            // Act & Assert
            Action assertion = ()
                => selector.Should().NotBeVirtual();

            assertion
                .Should()
                .Throw<Xunit.Sdk.XunitException>()
                .WithMessage("Expected property * not to be virtual, but it is.");
        }
    }
}
