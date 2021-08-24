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

        [Fact]
        public void When_selecting_virtual_properties_should_succceed()
        {
            // Arrange
            var testObj = new TestClass();
            var expectedPropertyInfos = typeof(TestClass)
                .GetPublicOrInternalProperties()
                .Where(propertyInfo => propertyInfo.GetMethod.IsVirtual);

            // Act
            var selectedProperties = testObj
                .Properties()
                .ThatAreVirtual;

            // Assert
            selectedProperties
                .Select(ipi => ipi.PropertyInfo)
                .Should()
                .BeEquivalentTo(expectedPropertyInfos);
        }

        [Fact]
        public void When_selecting_nonvirtual_properties_should_succceed()
        {
            // Arrange
            var testObj = new TestClass();
            var expectedPropertyInfos = typeof(TestClass)
                .GetPublicOrInternalProperties()
                .Where(propertyInfo => !propertyInfo.GetMethod.IsVirtual);

            // Act
            var selectedProperties = testObj
                .Properties()
                .ThatAreNotVirtual;

            // Assert
            selectedProperties
                .Select(ipi => ipi.PropertyInfo)
                .Should()
                .BeEquivalentTo(expectedPropertyInfos);
        }

        [Fact]
        public void When_selecting_read_only_properties_should_succceed()
        {
            // Arrange
            var testObj = new TestClass();
            var expectedPropertyInfos = typeof(TestClass)
                .GetPublicOrInternalProperties()
                .Where(propertyInfo => !propertyInfo.CanWrite);

            // Act
            var selectedProperties = testObj
                .Properties()
                .ThatAreReadOnly;

            // Assert
            selectedProperties
                .Select(ipi => ipi.PropertyInfo)
                .Should()
                .BeEquivalentTo(expectedPropertyInfos);
        }

        [Fact]
        public void When_selecting_writable_properties_should_succceed()
        {
            // Arrange
            var testObj = new TestClass();
            var expectedPropertyInfos = typeof(TestClass)
                .GetPublicOrInternalProperties()
                .Where(propertyInfo => propertyInfo.CanWrite);

            // Act
            var selectedProperties = testObj
                .Properties()
                .ThatAreWritable;

            // Assert
            selectedProperties
                .Select(ipi => ipi.PropertyInfo)
                .Should()
                .BeEquivalentTo(expectedPropertyInfos);
        }

        [Fact]
        public void When_selecting_properties_with_type_matching_custom_predicate_should_succceed()
        {
            // Arrange
            const string testPropertyName = "IntProperty";
            var testObj = new TestClass();
            var expectedPropertyInfo = typeof(TestClass)
                .GetProperty(testPropertyName);

            // Act
            var selectedProperties = testObj
                .Properties()
                .OfTypeMatching(propertyInfo => propertyInfo.Name == testPropertyName);

            // Assert
            selectedProperties
                .Select(ipi => ipi.PropertyInfo)
                .Should()
                .BeEquivalentTo(expectedPropertyInfo);
        }

        [Fact]
        public void When_selecting_properties_of_nullable_value_types_should_be_counted_for_value_types()
        {
            // Arrange
            const string nullablePropertyName = "NullableValueTypeProperty";
            var testObj = new TestClass();
            var expectedPropertyInfo = typeof(TestClass)
                .GetProperty(nullablePropertyName);

            // Act
            var selectedProperties = testObj
                .Properties()
                .ThatAreOfValueTypes;

            // Assert
            selectedProperties
                .Select(ipi => ipi.PropertyInfo)
                .Should()
                .Contain(expectedPropertyInfo);
        }

        [Fact]
        public void When_selecting_properties_that_are_not_internal_should_succeed()
        {
            // Arrange
            var testObj = new TestClass();
            var expectedPropertyInfos = typeof(TestClass)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Act
            var selectedProperties = testObj
                .Properties()
                .ThatAreNotInternal;

            // Assert
            selectedProperties
                .Select(ipi => ipi.PropertyInfo)
                .Should()
                .BeEquivalentTo(expectedPropertyInfos);
        }
        [Fact]
        public void When_selecting_properties_should_include_inheritted()
        {
            // Arrange
            const string inherittedPropertyName = "BaseClassProperty";
            var testObj = new TestClass();

            // Act
            var selectedProperties = testObj
                .Properties();

            // Assert
            selectedProperties
                .Select(ipi => ipi.PropertyInfo.Name)
                .Should()
                .Contain(inherittedPropertyName);
        }

        [Fact]
        public void When_selecting_properties_that_are_not_inheritted_should_succeed()
        {
            // Arrange
            const string inherittedPropertyName = "BaseClassProperty";
            var testObj = new TestClass();

            // Act
            var selectedProperties = testObj
                .Properties()
                .ThatAreNotInheritted;

            // Assert
            selectedProperties
                .Select(ipi => ipi.PropertyInfo.Name)
                .Should()
                .NotContain(inherittedPropertyName);
        }

        [Fact]
        public void When_selecting_properties_of_specific_types_should_succeed()
        {
            // Arrange
            var testObj = new TestClass();
            var expectedPropertyInfos = typeof(TestClass)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(propertyInfo => propertyInfo.PropertyType.Equals(typeof(string)));

            // Act
            var selectedProperties = testObj
                .Properties()
                .OfType<string>()
                .ThatAreNotInternal;

            // Assert
            selectedProperties
                .Select(ipi => ipi.PropertyInfo)
                .Should()
                .BeEquivalentTo(expectedPropertyInfos);
        }

        [Fact]
        public void When_selecting_properties_that_are_not_of_specific_types_should_succeed()
        {
            // Arrange
            var testObj = new TestClass();
            var expectedPropertyInfos = typeof(TestClass)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(propertyInfo => !propertyInfo.PropertyType.Equals(typeof(string)));

            // Act
            var selectedProperties = testObj
                .Properties()
                .NotOfType<string>();

            // Assert
            selectedProperties
                .Select(ipi => ipi.PropertyInfo)
                .Should()
                .BeEquivalentTo(expectedPropertyInfos);
        }

        private class EmptyTestClass
        { }

        private class TestClass : TestClassBase
        {
            public int IntProperty { get; set; }
            public double DoubleProperty { get; set; }
            public string StringProperty { get; set; }
            private string PrivateStringProperty { get; set; }
            internal string InternalStringProperty { get; set; }
            public string ReadOnlyStringProperty { get; }

            public int? NullableValueTypeProperty { get; set; }
            public EmptyTestClass UserTypeProperty { get; set; }

            public virtual bool VirtualProperty { get; set; }
        }

        public class TestClassBase
        {
            public bool BaseClassProperty { get; set; }
        }
    }


}
