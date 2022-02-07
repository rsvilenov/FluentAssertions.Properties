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
            InstancePropertySelector<TestClass> selector =
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
            InstancePropertySelector<TestClass> selector =
                testObj.Properties();

            // Act & Assert
            Action assertion = () 
                => selector.Should().HaveCount(expectedCount);

            assertion
                .Should()
                .Throw<Xunit.Sdk.XunitException>()
                .WithMessage($"Expected property count of type {typeof(TestClass)} to be {expectedCount}, but was not.");
        }
    }
}
