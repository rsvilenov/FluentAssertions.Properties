using FluentAssertions.Properties.Tests.PublicApi.TestObjects;
using FluentAssertions.Common;
using System;
using System.Linq;
using Xunit;

namespace FluentAssertions.Properties.Tests.PublicApi
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

        [Fact]
        public void When_selecting_all_read_only_properties_BeWritable_should_fail()
        {
            // Arrange
            var testObj = new TestClass();
            var selector =
                testObj.Properties().ThatAreReadOnly;

            // Act & Assert
            Action assertion = ()
                => selector.Should().BeWritable();

            assertion
                .Should()
                .Throw<Xunit.Sdk.XunitException>()
                .WithMessage("Expected property * to have a setter.");
        }

        [Fact]
        public void When_selecting_all_properties_with_internal_setter_BeWritable_with_internal_access_modifier_should_succeed()
        {
            // Arrange
            var testObj = new TestClass();
            var selector =
                testObj.Properties(p => p.StringPropertyWithInternalSetter);

            // Act & Assert
            selector.Should().BeWritable(CSharpAccessModifier.Internal);
        }

        [Fact]
        public void When_selecting_all_properties_with_internal_setter_BeWritable_with_public_access_modifier_should_fail()
        {
            // Arrange
            var testObj = new TestClass();
            var selector =
                testObj.Properties(p => p.StringPropertyWithInternalSetter);

            // Act & Assert
            Action assertion = ()
                => selector.Should().BeWritable(CSharpAccessModifier.Public);

            assertion
                .Should()
                .Throw<Xunit.Sdk.XunitException>()
                .WithMessage("Expected method * to be Public, but it is Internal.");
        }

        [Fact]
        public void When_selecting_all_properties_with_internal_setter_BeWritable_without_access_modifier_should_succeed()
        {
            // Arrange
            var testObj = new TestClass();
            var selector =
                testObj.Properties(p => p.StringPropertyWithInternalSetter);

            // Act & Assert
            selector.Should().BeWritable();
        }

        [Fact]
        public void When_selecting_all_properties_with_private_setter_BeWritable_with_public_access_modifier_should_fail()
        {
            // Arrange
            var testObj = new TestClass();
            var selector =
                testObj.Properties(p => p.StringPropertyWithPrivateSetter);

            // Act & Assert
            Action assertion = ()
                => selector.Should().BeWritable(CSharpAccessModifier.Public);

            assertion
                .Should()
                .Throw<Xunit.Sdk.XunitException>()
                .WithMessage("Expected method * to be Public, but it is Private.");
        }

        [Fact]
        public void When_selecting_all_properties_with_private_setter_BeWritable_without_access_modifier_should_succeed()
        {
            // Arrange
            var testObj = new TestClass();
            var selector =
                testObj.Properties(p => p.StringPropertyWithPrivateSetter);

            // Act & Assert
            selector.Should().BeWritable(); 
            // The property is still writable despite the fact that the setter is a private one.
            // This is how PropertyInfo.BeWritable() method of the original FluentAssertions library behaves
            // and this is also the behavior of reflection's PropertyInfo.CanWrite property.
        }
    }
}
