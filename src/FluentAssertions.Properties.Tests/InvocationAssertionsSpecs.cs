using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;

namespace FluentAssertions.Properties.Tests
{
    public class InvocationAssertionsSpecs
    {
        [Fact]
        public void When_selected_symmetric_properties_are_called_with_values_from_source_object_assert_should_succeed()
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

        public interface ITestProperties
        {
            string StringProperty { get; set; }
            int IntProperty { get; set; }
            string ReadOnlyStringProperty { get; }
            string WriteOnlyStringProperty { set; }
        }

        public class TestException : Exception
        { 
            public TestException(string message = null, Exception innerException = null) : base(message, innerException) { }
        }
    }
}
