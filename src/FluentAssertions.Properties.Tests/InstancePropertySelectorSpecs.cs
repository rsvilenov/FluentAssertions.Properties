using FluentAssertions.Properties.Selectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions.Properties.Extensions;
using System.Text;
using Xunit;

namespace FluentAssertions.Properties.Tests
{
    public class InstancePropertySelectorSpecs
    {
        [Fact]
        public void When_selecting_properties_of_a_class_it_should_not_throw()
        {
            // Arrange
            var testObj = new TestClass();
            Func<InstancePropertySelector<TestClass>> act = () =>
                testObj.Properties();

            // Act & Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_selecting_properties_of_a_class_should_succeed()
        {
            // Arrange
            var testObj = new TestClass();
            var expectedPropertyInfos = typeof(TestClass)
                .GetPublicOrInternalProperties();

            // Act
            var selectedProperties = testObj.Properties();

            // Assert
            selectedProperties
                .Select(ipi => ipi.PropertyInfo)
                .Should()
                .BeEquivalentTo(expectedPropertyInfos);
        }

        [Fact]
        public void When_selecting_properties_of_a_null_object_should_throw_ArgumentNullException()
        {
            // Arrange
            TestClass testObj = null;
            Action act = () => testObj.Properties();

            // Act & Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void When_selecting_from_class_with_no_properties_should_not_throw()
        {
            // Arrange
            var testObj = new EmptyTestClass();
            Action act = () => testObj.Properties();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_selecting_properties_of_primitive_types_should_succceed()
        {
            // Arrange
            var testObj = new TestClass();
            var expectedPropertyInfos = typeof(TestClass)
                .GetPublicOrInternalProperties()
                .Where(propertyInfo => propertyInfo.PropertyType.IsPrimitive);

            // Act
            var selectedProperties = testObj
                .Properties()
                .ThatAreOfPrimitiveTypes;

            // Assert
            selectedProperties
                .Select(ipi => ipi.PropertyInfo)
                .Should()
                .BeEquivalentTo(expectedPropertyInfos);
        }


        [Fact]
        public void When_selecting_properties_of_nonprimitive_types_should_succceed()
        {
            // Arrange
            var testObj = new TestClass();
            var expectedPropertyInfos = typeof(TestClass)
                .GetPublicOrInternalProperties()
                .Where(propertyInfo => !propertyInfo.PropertyType.IsPrimitive);

            // Act
            var selectedProperties = testObj
                .Properties()
                .ThatAreNotOfPrimitiveTypes;

            // Assert
            selectedProperties
                .Select(ipi => ipi.PropertyInfo)
                .Should()
                .BeEquivalentTo(expectedPropertyInfos);
        }


        [Fact]
        public void When_selecting_properties_of_value_types_should_succceed()
        {
            // Arrange
            var testObj = new TestClass();
            var expectedPropertyInfos = typeof(TestClass)
                .GetPublicOrInternalProperties()
                .Where(propertyInfo => propertyInfo.PropertyType.IsValueType);

            // Act
            var selectedProperties = testObj
                .Properties()
                .ThatAreOfValueTypes;

            // Assert
            selectedProperties
                .Select(ipi => ipi.PropertyInfo)
                .Should()
                .BeEquivalentTo(expectedPropertyInfos);
        }


        [Fact]
        public void When_selecting_properties_of_reference_types_should_succceed()
        {
            // Arrange
            var testObj = new TestClass();
            var expectedPropertyInfos = typeof(TestClass)
                .GetPublicOrInternalProperties()
                .Where(propertyInfo => !propertyInfo.PropertyType.IsValueType);

            // Act
            var selectedProperties = testObj
                .Properties()
                .ThatAreOfReferenceTypes;

            // Assert
            selectedProperties
                .Select(ipi => ipi.PropertyInfo)
                .Should()
                .BeEquivalentTo(expectedPropertyInfos);
        }

        private class EmptyTestClass
        { }

        private class TestClass
        {
            public int IntProperty { get; set; }
            public double DoubleProperty { get; set; }
            public string StringProperty { get; set; }
            private string PrivateStringProperty { get; set; }
            internal string InternalStringProperty { get; set; }
            public string ReadOnlyStringProperty { get; }

            public EmptyTestClass UserTypeProperty { get; set; }
        }
    }


}
