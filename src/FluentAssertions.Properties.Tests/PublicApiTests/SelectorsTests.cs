﻿using FluentAssertions.Properties.Extensions;
using FluentAssertions.Properties.Selectors;
using FluentAssertions.Properties.Tests.Extensions;
using FluentAssertions.Properties.Tests.PublicApiTests.TestObjects;
using Moq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Xunit;

namespace FluentAssertions.Properties.Tests.PublicApiTests
{
    public class SelectorsTests
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
        public void When_selecting_specific_properties_of_known_type_it_should_not_throw()
        {
            // Arrange
            var testObj = new TestClass();
            Func<InstancePropertyOfKnownTypeSelector<TestClass, string>> act = () =>
                testObj.Properties(p => p.StringProperty);

            // Act & Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_selecting_properties_from_null_object_it_shoud_throw()
        {
            // Arrange
            TestClass testObj = null;
            Func<InstancePropertySelector<TestClass>> act = () =>
                testObj.Properties();

            // Act & Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void When_selecting_specific_properties_from_null_object_it_shoud_throw()
        {
            // Arrange
            TestClass testObj = null;
            Func<InstancePropertyOfKnownTypeSelector<TestClass, string>> act = () =>
                testObj.Properties(p => p.StringProperty);

            // Act & Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void When_selecting_specific_properties_of_known_type_selected_properties_should_match_the_expected_ones()
        {
            // Arrange
            var testObj = new TestClass();

            // Act
            var selectedProperties = testObj
                .Properties(p => p.StringProperty);

            // Assert
            selectedProperties
                .GetSelection()
                .Select(ipi => ipi.PropertyInfo.Name)
                .Should()
                .HaveCount(1)
                .And
                .ContainSingle(nameof(TestClass.StringProperty));
        }

        [Fact]
        public void When_selecting_properties_of_known_type_from_class_with_no_properties_selected_property_collection_should_be_empty()
        {
            // Arrange
            var testObj = new EmptyTestClass();

            // Act
            var selectedProperties = testObj
                .Properties<EmptyTestClass, string>();

            // Assert
            selectedProperties
                ?.Count()
                .Should()
                .Be(0);
        }

        [Fact]
        public void When_selecting_properties_of_known_type_from_class_property_collection_should_match_expected()
        {
            // Arrange
            var testObj = new TestClass();
            var expectedPropertyInfos = typeof(TestClass)
                .GetPublicOrInternalProperties()
                .Where(pi => pi.PropertyType == typeof(string));

            // Act
            var selectedProperties = testObj
                .Properties<TestClass, string>();

            // Assert
            selectedProperties
                .GetSelection()
                .Select(ipi => ipi.PropertyInfo)
                .Should()
                .BeEquivalentTo(expectedPropertyInfos);
        }

        [Fact]
        public void When_selecting_specific_properties_of_various_types_selected_properties_should_match_the_expected_ones()
        {
            // Arrange
            var testObj = new TestClass();

            // Act
            var selectedPropertyNames = testObj
                .Properties(p => p.StringProperty, p => p.IntProperty)
                .GetSelection()
                .Select(ipi => ipi.PropertyInfo.Name);

            // Assert
            selectedPropertyNames
                .Should()
                .HaveCount(2)
                .And
                .Contain(nameof(TestClass.StringProperty), nameof(TestClass.StringProperty));
        }

        [Fact]
        public void When_selecting_specific_properties_of_various_types_it_should_not_throw()
        {
            // Arrange
            var testObj = new TestClass();
            Func<InstancePropertySelector<TestClass>> act = () =>
                testObj.Properties(p => p.StringProperty, p => p.IntProperty);

            // Act & Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_selecting_properties_with_invalid_expression_type_should_throw()
        {
            // Arrange
            var testObj = new TestClass();
            Expression<Func<TestClass, object>> unsupportedExpression = (a) => null;
            Func<InstancePropertySelector<TestClass>> act = () =>
                testObj.Properties<TestClass>(unsupportedExpression);

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
                .GetSelection()
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

            // Act & Assert
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
                .GetSelection()
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
                .GetSelection()
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
                .GetSelection()
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
                .GetSelection()
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
                .GetSelection()
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
                .GetSelection()
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
                .GetSelection()
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
                .GetSelection()
                .Select(ipi => ipi.PropertyInfo)
                .Should()
                .BeEquivalentTo(expectedPropertyInfos);
        }

#if NET5_0_OR_GREATER
        [Fact]
        public void When_selecting_init_only_properties_should_succceed()
        {
            // Arrange
            var testObj = new TestRecord();
            var expectedPropertyName = nameof(TestRecord.MyInitOnlyProperty);

            // Act
            var selectedProperties = testObj
                .Properties()
                .ThatAreInitOnly;

            // Assert
            selectedProperties
                .GetSelection()
                .Select(ipi => ipi.PropertyInfo.Name)
                .Should()
                .ContainSingle(expectedPropertyName);
        }
#endif

        [Fact]
        public void When_selecting_properties_with_type_matching_custom_predicate_should_succceed()
        {
            // Arrange
            const string testPropertyName = nameof(TestClass.IntProperty);
            var testObj = new TestClass();
            var expectedPropertyInfo = typeof(TestClass)
                .GetProperty(testPropertyName);

            // Act
            var selectedProperties = testObj
                .Properties()
                .OfTypeMatching(propertyInfo => propertyInfo.Name == testPropertyName);

            // Assert
            selectedProperties
                .GetSelection()
                .Select(ipi => ipi.PropertyInfo)
                .Should()
                .ContainSingle()
                .And
                .ContainEquivalentOf(expectedPropertyInfo);
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
                .GetSelection()
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
                .GetSelection()
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
                .GetSelection()
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
                .GetSelection()
                .Select(ipi => ipi.PropertyInfo.Name)
                .Should()
                .NotContain(inherittedPropertyName);
        }

        [Fact]
        public void When_selecting_properties_castable_to_specified_type_it_should_succeed()
        {
            // Arrange
            var testObj = new TestClassPublicPropertiesOnly();
            var expectedPropertyInfos = typeof(TestClassPublicPropertiesOnly)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(propertyInfo => typeof(string).IsAssignableFrom(propertyInfo.PropertyType));
            
            // Act
            var selectedProperties = testObj
                .Properties()
                .OfType<string>();

            // Assert
            selectedProperties
                .GetSelection()
                .Select(ipi => ipi.PropertyInfo)
                .Should()
                .BeEquivalentTo(expectedPropertyInfos);
        }

        [Fact]
        public void When_selecting_properties_castable_to_object_OfType_should_return_all_properties()
        {
            // Arrange
            var testObj = new TestClassPublicPropertiesOnly();
            var expectedPropertyCount = typeof(TestClassPublicPropertiesOnly)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Count();

            // Act
            var selectedProperties = testObj
                .Properties()
                .OfType<object>();

            // Assert
            selectedProperties
                .Should()
                .HaveCount(expectedPropertyCount);
        }

        [Fact]
        public void When_selecting_properties_of_derived_types_ExactlyOfType_with_object_type_param_should_return_no_properties()
        {
            // Arrange
            var testObj = new TestClassPublicPropertiesOnly();

            // Act
            var selectedProperties = testObj
                .Properties(p => p.IntProperty, p => p.StringProperty)
                .ExactlyOfType<object>();

            // Assert
            selectedProperties
                .Should()
                .HaveCount(0);
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
                .ExactlyOfType<string>();

            // Assert
            selectedProperties
                .GetSelection()
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
                .GetSelection()
                .Select(ipi => ipi.PropertyInfo)
                .Should()
                .BeEquivalentTo(expectedPropertyInfos);
        }

        [Fact]
        public void When_selecting_properties_NotOfType_with_object_type_param_should_not_return_any_properties()
        {
            // Arrange
            var testObj = new TestClass();

            // Act
            var selectedProperties = testObj
                .Properties()
                .NotOfType<object>();

            // Assert
            selectedProperties
                .Should()
                .HaveCount(0);
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
                .ExactlyOfType<string>()
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
                .GetSelection();

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
            var selectedProperties = testObj
                .Properties();

            // Assert
            selectedProperties
                .GetSelection()
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
            var selectedProperties = testObj
                .Properties()
                .ThatAreOfValueType
                .ThatAreNullable;

            // Assert
            selectedProperties
                .GetSelection()
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
            var selectedProperties = testObj
                .Properties()
                .ThatAreOfValueType
                .ThatAreNotNullable;

            // Assert
            selectedProperties
                .GetSelection()
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
            var selectedProperties = testObj
                .Properties()
                .ThatAreOfValueType
                .ThatHaveDefaultValue;

            // Assert
            selectedProperties
                .GetSelection()
                .Select(ipi => ipi.PropertyInfo.Name)
                .Should()
                .NotContain(s => s.EndsWith(TestClassValueTypePropertiesOnly.NonDefaultValueSuffix));
        }

        [Fact]
        public void When_selecting_properties_of_value_types_that_have_default_value_and_throwing_getters_should_fail_with_appropriate_message()
        {
            // Arrange
            string exceptionMessage = Guid.NewGuid().ToString();
            var testPropertyMock = new Mock<ITestProperties>();
            testPropertyMock
                .SetupGet(o => o.IntProperty)
                .Throws(new TestException(exceptionMessage));

            // Act & Assert
            var testAction = () => testPropertyMock.Object
                .Properties()
                .ThatAreOfValueType
                .ThatHaveDefaultValue;

            testAction
                .Should()
                .Throw<Xunit.Sdk.XunitException>()
                .WithMessage($"Did not expect any exceptions for property *, but got * {exceptionMessage}*");
        }

        [Fact]
        public void When_selecting_properties_of_string_type_that_have_no_value_HavingValue_should_return_no_properties()
        {
            // Arrange
            var testObj = new TestClass();
            string testValue = Guid.NewGuid().ToString();

            // Act
            var selectedProperties = testObj
                .Properties()
                .ExactlyOfType<string>()
                .HavingValue(testValue);

            // Assert
            var selectedPropertyNames = selectedProperties
                .GetSelection()
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
            var selectedProperties = testObj
                .Properties()
                .ExactlyOfType<string>()
                .HavingValue(testValue);

            // Assert
            var selectedPropertyNames = selectedProperties
                .GetSelection()
                .Select(ipi => ipi.PropertyInfo.Name);

            selectedPropertyNames
                .Should()
                .HaveCount(1)
                .And
                .ContainSingle(p => p == nameof(testObj.StringProperty));
        }

        [Fact]
        public void When_selecting_properties_of_string_type_that_have_no_value_HavingValue_with_null_should_succeed()
        {
            // Arrange
            var testObj = new TestClass();
            string testValue = null;
            testObj.StringProperty = "not null";

            // Act
            var selectedProperties = testObj
                .Properties()
                .ExactlyOfType<string>()
                .HavingValue(testValue);

            // Assert
            var selectedPropertyNames = selectedProperties
                .GetSelection()
                .Select(ipi => ipi.PropertyInfo.Name);

            selectedPropertyNames
                .Should()
                .NotBeEmpty()
                .And
                .NotContain(p => p == nameof(testObj.StringProperty));
        }

        [Fact]
        public void When_selecting_properties_of_value_types_that_have_throwing_getters_HavingValue_should_fail_with_appropriate_message()
        {
            // Arrange
            string exceptionMessage = Guid.NewGuid().ToString();
            var testPropertyMock = new Mock<ITestProperties>();
            testPropertyMock
                .SetupGet(o => o.IntProperty)
                .Throws(new TestException(exceptionMessage));

            // Act & Assert
            var testAction = () => testPropertyMock.Object
                .Properties(o => o.IntProperty)
                .HavingValue(1);

            testAction
                .Should()
                .Throw<Xunit.Sdk.XunitException>()
                .WithMessage($"Did not expect any exceptions for property *, but got * {exceptionMessage}*");
        }

        [Fact]
        public void When_using_Properties_selector_over_a_type_and_not_an_instance_it_should_succeed()
        {
            var selector = typeof(TestClass).Properties();
            selector
                .ToArray()
                .Should()
                .NotBeNullOrEmpty()
                .And
                .Contain(m => m.Name == nameof(TestClass.StringProperty));
        }

        [Fact]
        public void When_using_Properties_selector_on_type_selector_it_should_succeed()
        {
            // Arrange
            Assembly assembly = Assembly.GetExecutingAssembly();

            // Act
            var selector = assembly
                .Types()
                .Properties();

            selector
                .ToArray()
                .Should()
                .NotBeNullOrEmpty()
                .And
                .Contain(m => m.Name == nameof(TestClass.StringProperty));
        }
    }
}
