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
        public void When_selecting_properties_of_a_class_it_should_succeed()
        {
            // Arrange
            var testObj = new TestClass();

            // Act
            Func<InstancePropertySelector<TestClass>> act = () =>
                testObj.Properties();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_selecting_properties_of_a_class_count_should_match()
        {
            // Arrange
            var testObj = new TestClass();
            int expectedPublicOrInternalPropertyCount = typeof(TestClass)
                .GetPublicOrInternalProperties()
                .Count();

            // Act
            int gotPropertyCount = testObj.Properties().Count();

            // Assert
            gotPropertyCount.Should().Be(expectedPublicOrInternalPropertyCount);
        }

        [Fact]
        public void When_selecting_properties_of_a_null_object_should_throw_ArgumentNullException()
        {
            // Arrange
            TestClass testObj = null;

            // Act
            Action act = () => testObj.Properties();

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        private class TestClass
        {
            public int IntProperty { get; set; }
            public double DoubleProperty { get; set; }
            public string StringProperty { get; set; }
            private string PrivateStringProperty { get; set; }
            internal string InternalStringProperty { get; set; }
            public string ReadOnlyStringProperty { get; }
        }
    }


}
