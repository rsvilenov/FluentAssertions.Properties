using Moq;
using System;
using Xunit;
using Xunit.Sdk;
using FluentAssertions.Properties.Tests.PublicApiTests.TestObjects;
using FluentAssertions.Properties.Tests.Extensions;

namespace FluentAssertions.Properties.Tests.PublicApiTests
{
    public partial class InvocationAssertionsTests : PublicApiTestBase
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
        public void When_selected_symmetric_properties_are_called_with_null_ProvideSymmetricAccess_assert_should_succeed()
        {
            // Arrange
            var testObj = Mock.Of<ITestProperties>();
            var valueSourceMock = new Mock<ITestProperties>();;
            var symmetricProperties = testObj
                .Properties(p => p.StringProperty);

            // Act & Assert
            symmetricProperties
                .WhenCalledWithValuesFrom(valueSourceMock.Object)
                .Should()
                .ProvideSymmetricAccess();
        }

        [Fact]
        public void When_selected_asymmetric_properties_are_called_with_values_from_source_object_ProvideSymmetricAccess_assert_should_fail()
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

            var assertReason = base.CreateAssertReason();

            // Act & Assert
            Action action = () =>
                symmetricProperties
                    .WhenCalledWithValuesFrom(valueSourceMock.Object)
                    .Should()
                    .ProvideSymmetricAccess(assertReason.BecauseWithFormat, assertReason.BecauseArg1, assertReason.BecauseArg2);

            action
                .Should()
                .Throw<XunitException>()
                .WithMessage($"Expected the get and set operations of property * to be symmetric because {assertReason}, but were not.");
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

            var assertReason = base.CreateAssertReason();

            // Act & Assert
            Action action = () =>
                symmetricProperties
                    .WhenCalledWithValuesFrom(valueSourceMock.Object)
                    .Should()
                    .ProvideSymmetricAccess(assertReason.BecauseWithFormat, assertReason.BecauseArg1, assertReason.BecauseArg2);

            action
                .Should()
                .Throw<XunitException>()
                .WithMessage($"Expected property * to be writable because {assertReason}, but was not.");
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
                .WithMessage("Did not expect any exceptions for property *, but *");
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

            var assertReason = base.CreateAssertReason();

            // Act & Assert
            Action assertion = () => symmetricProperties
                .WhenCalledWithValuesFrom(valueSourceMock.Object)
                .Should()
                .ThrowFromGetter<TestException>(assertReason.BecauseWithFormat, assertReason.BecauseArg1, assertReason.BecauseArg2);

            assertion
                .Should()
                .Throw<XunitException>()
                .WithMessage($"Expected property \"getter\" of property * to throw * because {assertReason}*");
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

            var assertReason = base.CreateAssertReason();

            // Act & Assert
            Action assertion = () => symmetricProperties
                .WhenCalledWithValuesFrom(valueSourceMock.Object)
                .Should()
                .ThrowFromGetterExactly<Exception>(assertReason.BecauseWithFormat, assertReason.BecauseArg1, assertReason.BecauseArg2);

            assertion
                .Should()
                .Throw<XunitException>()
                .WithMessage($"Expected property \"getter\" of property * to throw * because {assertReason}*");
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

            var assertReason = base.CreateAssertReason();

            // Act & Assert
            Action assertion = () => symmetricProperties
                .WhenCalledWithValuesFrom(valueSourceMock.Object)
                .Should()
                .ThrowFromSetterExactly<Exception>(assertReason.BecauseWithFormat, assertReason.BecauseArg1, assertReason.BecauseArg2);

            assertion
                .Should()
                .Throw<XunitException>()
                .WithMessage($"Expected property \"setter\" of property * to throw * because {assertReason}*");
        }

        [Fact]
        public void When_the_source_object_throws_an_exception_ThrowFromSetter_assert_should_fail()
        {
            // Arrange
            var testPropertyMock = new Mock<ITestProperties>();

            var valueSourceMock = new Mock<ITestProperties>();
            valueSourceMock
                .SetupGet(o => o.StringProperty)
                .Throws<TestException>();

            var symmetricProperties = testPropertyMock
                .Object
                .Properties(p => p.StringProperty);

            // Act & Assert
            Action assertion = () => symmetricProperties
                .WhenCalledWithValuesFrom(valueSourceMock.Object)
                .Should()
                .ThrowFromSetter<Exception>();

            assertion
                .Should()
                .Throw<XunitException>()
                .WithMessage($"Did not expect any exceptions when getting the value to be passed to property *{nameof(TestException)} * was thrown*");
        }

        [Fact]
        public void When_selecting_properties_of_string_type_WhenCalledWith_ProvideSymmetricAccess_assert_should_succeed()
        {
            // Arrange
            var testObj = new TestClassPublicPropertiesOnly();
            string testValue = Guid.NewGuid().ToString();
            testObj.StringProperty = testValue;

            // Act & Assert
            testObj
                .Properties()
                .ThatAreWritable
                .OfType<string>()
                .WhenCalledWith(testValue)
                .Should()
                .ProvideSymmetricAccess();
        }

        [Fact]
        public void When_the_source_object_throws_an_exception_ProvideSymmetricAccess_assert_should_fail()
        {
            // Arrange
            var testPropertyMock = new Mock<ITestProperties>();

            var valueSourceMock = new Mock<ITestProperties>();
            valueSourceMock
                .SetupGet(o => o.StringProperty)
                .Throws<TestException>();

            var symmetricProperties = testPropertyMock
                .Object
                .Properties(p => p.StringProperty);

            // Act & Assert
            Action assertion = () => symmetricProperties
                .WhenCalledWithValuesFrom(valueSourceMock.Object)
                .Should()
                .ProvideSymmetricAccess();

            assertion
                .Should()
                .Throw<XunitException>()
                .WithMessage($"Did not expect any exceptions when getting the value to be passed to property *{nameof(TestException)} * was thrown*");
        }

        [Fact]
        public void When_selected_properties_throw_from_setter_ProvideSymmetricAccess_assert_should_fail()
        {
            // Arrange
            var testPropertyMock = new Mock<ITestProperties>();
            string guid = Guid.NewGuid().ToString();
            testPropertyMock
                .SetupSet(o => o.StringProperty = guid)
                .Throws<TestException>();
            testPropertyMock
                .SetupSet(o => o.IntProperty = 0)
                .Throws<TestException>();

            var valueSourceMock = new Mock<ITestProperties>();
            valueSourceMock
                .Setup(o => o.StringProperty)
                .Returns(guid);

            var symmetricProperties = testPropertyMock
                .Object
                .Properties(p => p.StringProperty, p => p.IntProperty);

            // Act & Assert
            Action assertion = () => symmetricProperties
                .WhenCalledWithValuesFrom(valueSourceMock.Object)
                .Should()
                .ProvideSymmetricAccess();

            assertion
                .Should()
                .Throw<XunitException>()
                .WithMessage($"Did not expect any exceptions for property {nameof(ITestProperties.StringProperty)}, but the setter threw *{nameof(TestException)}*" +
                    $"Did not expect any exceptions for property {nameof(ITestProperties.IntProperty)}, but the setter threw *{nameof(TestException)}*");
        }

        [Fact]
        public void When_selecting_a_single_property_WhenCalledWith_ThrowFromSetter_assert_should_fail()
        {
            // Arrange
            var testPropertyMock = new Mock<ITestProperties>();
            string guid = Guid.NewGuid().ToString();
            testPropertyMock
                .SetupSet(o => o.StringProperty = guid)
                .Throws<TestException>();

            // Act & Assert
            testPropertyMock
                .Object
                .Properties(p => p.StringProperty)
                .WhenCalledWith(guid)
                .Should()
                .ThrowFromSetter<TestException>();
        }

        [Fact]
        public void When_selected_properties_do_not_throw_from_setter_ThrowFromSetter_assert_should_return_messages_for_all_properties_expected_to_fail()
        {
            // Arrange
            var testPropertyMock = new Mock<ITestProperties>();

            var valueSourceMock = new Mock<ITestProperties>();

            var symmetricProperties = testPropertyMock
                .Object
                .Properties(p => p.StringProperty, p => p.IntProperty);

            // Act & Assert
            Action assertion = () => symmetricProperties
                .WhenCalledWithValuesFrom(valueSourceMock.Object)
                .Should()
                .ThrowFromSetter<TestException>();

            assertion
                .Should()
                .Throw<XunitException>()
                .WithMessage($"*{nameof(ITestProperties.StringProperty)}*{nameof(ITestProperties.IntProperty)}*");
        }

        [Fact]
        public void When_selected_properties_throw_from_setter_ThrowFromSetter_assert_should_return_messages_for_all_failed_properties()
        {
            // Arrange
            var testPropertyMock = new Mock<ITestProperties>();
            string guid = Guid.NewGuid().ToString();
            testPropertyMock
                .SetupSet(o => o.StringProperty = guid)
                .Throws<TestException>();
            testPropertyMock
                .SetupSet(o => o.IntProperty = 0)
                .Throws<TestException>();

            var valueSourceMock = new Mock<ITestProperties>();
            valueSourceMock
                .Setup(o => o.StringProperty)
                .Returns(guid);

            var symmetricProperties = testPropertyMock
                .Object
                .Properties(p => p.StringProperty, p => p.IntProperty);

            // Act & Assert
            Action assertion = () => symmetricProperties
                .WhenCalledWithValuesFrom(valueSourceMock.Object)
                .Should()
                .ThrowFromSetter<NullReferenceException>();

            assertion
                .Should()
                .Throw<XunitException>()
                .WithMessage($"*{nameof(ITestProperties.StringProperty)}*{typeof(NullReferenceException).Name}*{typeof(TestException).Name}*" +
                    $"*{nameof(ITestProperties.IntProperty)}*{typeof(NullReferenceException).Name}*{typeof(TestException).Name}*");
        }

        [Fact]
        public void When_selecting_properties_WhenCalledWith_count_with_nongeneric_enumerable_should_succeed()
        {
            // Arrange
            var testPropertyMock = new Mock<ITestProperties>();
            string guid = Guid.NewGuid().ToString();

            // Act & Assert
            testPropertyMock
                .Object
                .Properties(p => p.StringProperty)
                .WhenCalledWith(guid)
                .AsNonGenericEnumerable()
                .Count()
                .Should()
                .Be(1);
        }
    }
}
