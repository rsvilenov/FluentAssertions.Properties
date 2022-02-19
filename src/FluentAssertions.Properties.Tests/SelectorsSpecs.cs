using FluentAssertions.Properties.Extensions;
using FluentAssertions.Properties.Selectors;
using FluentAssertions.Properties.Tests.Extensions;
using FluentAssertions.Properties.Tests.TestObjects;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Xunit;

namespace FluentAssertions.Properties.Tests
{
    public partial class SelectorsSpecs
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
        public void When_selecting_properties_with_invalid_expression_type_should_throw()
        {
            Expression<Func<TestClass, object>> unsupportedExpression = (a) => null;

            // Arrange
            var testObj = new TestClass();
            Func<InstancePropertySelector<TestClass>> act = () =>
                testObj.Properties(unsupportedExpression);

            // Act & Assert
            act.Should().Throw<NotSupportedException>();
        }

        [Fact]
        public void When_selecting_properties_of_a_class_selection_should_succeed()
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
        public void When_selecting_properties_of_a_class_count_via_nongeneric_enumerable_should_succeed()
        {
            // Arrange
            var testObj = new TestClass();
            var expectedPropertyCount = typeof(TestClass)
                .GetPublicOrInternalProperties()
                .Count();

            // Act
            int propertyCountViaNonGenericEnumerable = testObj
                .Properties()
                .AsNonGenericEnumerable()
                .Count();

            // Assert
            propertyCountViaNonGenericEnumerable
                .Should()
                .Be(expectedPropertyCount);
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
                .ThatAreOfValueType;

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
                .ThatAreOfReferenceType;

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
                .ThatAreOfValueType;

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
            var testObj = new TestSubClass();

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
            var testObj = new TestClassPublicPropertiesOnly();
            var expectedPropertyInfos = typeof(TestClassPublicPropertiesOnly)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(propertyInfo => propertyInfo.PropertyType.Equals(typeof(string)));

            // Act
            var selectedProperties = testObj
                .Properties()
                .OfType<string>();

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

        [Fact]
        public void When_selecting_properties_return_types_method_count_should_match_test_object_public_property_count()
        {
            // Arrange
            var testObj = new TestClassPublicPropertiesOnly();
            var expectedCount = typeof(TestClassPublicPropertiesOnly)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Length;

            // Act
            var selectedPropertyReturnTypes = testObj
                .Properties()
                .ReturnTypes();

            // Assert
            selectedPropertyReturnTypes
                .Count()
                .Should()
                .Be(expectedCount);
        }

        [Fact]
        public void When_selecting_properties_return_types_methods_first_type_should_match_first_property_type_of_test_object()
        {
            // Arrange
            var testObj = new TestClass();

            // Act
            var firstPropertyReturnType = testObj
                .Properties()
                .OfType<string>()
                .ReturnTypes()
                .First();

            // Assert
            firstPropertyReturnType
                .Should()
                .Be(typeof(string));
        }

        [Fact]
        public void When_selecting_properties_to_array_count_should_match_test_object_public_property_types()
        {
            // Arrange
            var testObj = new TestClassPublicPropertiesOnly();
            var expectedPropertyTypes = typeof(TestClassPublicPropertiesOnly)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(pi => pi.PropertyType);

            // Act
            var selectedPropertyArray = testObj
                .Properties()
                .ToArray();

            // Assert
            selectedPropertyArray
                .Select(ipi => ipi.PropertyInfo.PropertyType)
                .Should()
                .BeEquivalentTo(expectedPropertyTypes);
        }

        [Fact]
        public void When_selecting_properties_is_used_as_enumerable_its_result_should_match_test_object_public_property_types()
        {
            // Arrange
            var testObj = new TestClassPublicPropertiesOnly();
            var expectedPropertyTypes = typeof(TestClassPublicPropertiesOnly)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(pi => pi.PropertyType);

            // Act
            var selectedPropertyList = testObj
                .Properties()
                .ToList();

            // Assert
            selectedPropertyList
                .Select(ipi => ipi.PropertyInfo.PropertyType)
                .Should()
                .BeEquivalentTo(expectedPropertyTypes);
        }

        [Fact]
        public void When_selecting_properties_of_value_types_that_are_nullable_should_succeed()
        {
            // Arrange
            var testObj = new TestClassValueTypePropertiesOnly();
            var expectedPropertyTypes = typeof(TestClassValueTypePropertiesOnly)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(pi => Nullable.GetUnderlyingType(pi.PropertyType) != null)
                .Select(pi => pi.PropertyType);

            // Act
            var selectedPropertyList = testObj
                .Properties()
                .ThatAreOfValueType
                .ThatAreNullable;

            // Assert
            selectedPropertyList
                .Select(ipi => ipi.PropertyInfo.PropertyType)
                .Should()
                .BeEquivalentTo(expectedPropertyTypes);
        }

        [Fact]
        public void When_selecting_properties_of_value_types_that_are_not_nullable_should_succeed()
        {
            // Arrange
            var testObj = new TestClassValueTypePropertiesOnly();
            var expectedPropertyTypes = typeof(TestClassValueTypePropertiesOnly)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(pi => Nullable.GetUnderlyingType(pi.PropertyType) == null)
                .Select(pi => pi.PropertyType);

            // Act
            var selectedPropertyList = testObj
                .Properties()
                .ThatAreOfValueType
                .ThatAreNotNullable;

            // Assert
            selectedPropertyList
                .Select(ipi => ipi.PropertyInfo.PropertyType)
                .Should()
                .BeEquivalentTo(expectedPropertyTypes);
        }

        [Fact]
        public void When_selecting_properties_of_value_types_that_have_default_value_should_succeed()
        {
            // Arrange
            var testObj = new TestClassValueTypePropertiesOnly();

            // Act
            var selectedPropertyList = testObj
                .Properties()
                .ThatAreOfValueType
                .ThatHaveDefaultValue;

            // Assert
            selectedPropertyList
                .Select(ipi => ipi.PropertyInfo.Name)
                .Should()
                .NotContain(s => s.EndsWith(TestClassValueTypePropertiesOnly.NonDefaultValueSuffix));
        }

        [Fact]
        public void When_selecting_properties_of_string_type_that_have_no_value_HavingValue_should_return_no_properties()
        {
            // Arrange
            var testObj = new TestClass();
            string testValue = Guid.NewGuid().ToString();

            // Act
            var selectedPropertyList = testObj
                .Properties()
                .OfType<string>()
                .HavingValue(testValue);

            // Assert
            var selectedPropertyNames = selectedPropertyList
                .Select(ipi => ipi.PropertyInfo.Name);

            selectedPropertyNames
                .Should()
                .HaveCount(0);
        }

        [Fact]
        public void When_selecting_properties_of_string_type_that_have_value_HavingValue_should_succeed()
        {
            // Arrange
            var testObj = new TestClass();
            string testValue = Guid.NewGuid().ToString();
            testObj.StringProperty = testValue;

            // Act
            var selectedPropertyList = testObj
                .Properties()
                .OfType<string>()
                .HavingValue(testValue);

            // Assert
            var selectedPropertyNames = selectedPropertyList
                .Select(ipi => ipi.PropertyInfo.Name);

            selectedPropertyNames
                .Should()
                .HaveCount(1);

            selectedPropertyNames
                .Single()
                .Should()
                .Be(nameof(testObj.StringProperty));
        }

    }
}
