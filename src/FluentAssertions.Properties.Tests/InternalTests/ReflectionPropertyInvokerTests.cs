using FluentAssertions.Properties.Invocation;
using System;
using Xunit;

namespace FluentAssertions.Properties.Tests.InternalTests
{
    public class ReflectionPropertyInvokerTests
    {
        public class PropertyContainer
        {
            public int ReadWriteProperty { get; set; }
            public int ReadOnlyProperty { get; }

            private int _writeOnlyProperty;
            public int WriteOnlyProperty { set { _writeOnlyProperty = value; } }

            public int GetWriteOnlyPropertyValue()
            {
                return _writeOnlyProperty;
            }

        }

        [Fact]
        public void When_setting_value_to_property_it_should_succeed()
        {
            // Arrange
            int testValue = 2;
            var instance = new PropertyContainer();
            var invoker = new ReflectionPropertyInvoker<PropertyContainer, int>(instance);

            // Act
            invoker.SetValue(nameof(PropertyContainer.ReadWriteProperty), testValue);

            // Assert
            instance.ReadWriteProperty.Should().Be(testValue);
        }

        [Fact]
        public void When_setting_value_to_property_without_setter_it_should_throw_argument_exception()
        {
            // Arrange
            int testValue = 2;
            var instance = new PropertyContainer();
            var invoker = new ReflectionPropertyInvoker<PropertyContainer, int>(instance);

            // Act
            Action testAction = () => invoker.SetValue(nameof(PropertyContainer.ReadOnlyProperty), testValue);

            // Assert
            testAction.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void When_setting_value_to_nonexisting_property_it_should_throw_argument_exception()
        {
            // Arrange
            int testValue = 2;
            var instance = new PropertyContainer();
            var invoker = new ReflectionPropertyInvoker<PropertyContainer, int>(instance);

            // Act
            Action testAction = () => invoker.SetValue("NonExistingProperty", testValue);

            // Assert
            testAction.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void When_getting_value_from_property_it_should_succeed()
        {
            // Arrange
            int testValue = 2;
            var instance = new PropertyContainer()
            {
                ReadWriteProperty = testValue
            };
            var invoker = new ReflectionPropertyInvoker<PropertyContainer, int>(instance);

            // Act
            int gotValue = invoker.GetValue(nameof(PropertyContainer.ReadWriteProperty));

            // Assert
            gotValue.Should().Be(testValue);
        }

        [Fact]
        public void When_getting_value_from_property_without_getter_it_should_throw_argument_exception()
        {
            // Arrange
            var instance = new PropertyContainer();
            var invoker = new ReflectionPropertyInvoker<PropertyContainer, int>(instance);

            // Act
            Action testAction = () => invoker.GetValue(nameof(PropertyContainer.WriteOnlyProperty));

            // Assert
            testAction.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void When_getting_value_from_nonexisting_property_it_should_throw_argument_exception()
        {
            // Arrange
            var instance = new PropertyContainer();
            var invoker = new ReflectionPropertyInvoker<PropertyContainer, int>(instance);

            // Act
            Action testAction = () => invoker.GetValue("NonExistingProperty");

            // Assert
            testAction.Should().Throw<ArgumentException>();
        }
    }
}
