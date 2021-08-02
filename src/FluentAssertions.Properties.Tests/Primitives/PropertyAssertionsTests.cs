using FluentAssertions.Properties.Tests.TestableObjects;
using System;
using System.Linq;
using Xunit;

namespace FluentAssertions.Properties.Primitives
{
    public class PropertyAssertionsTests
    {
        [Fact]
        public void Test1()
        {
            // Arrange
            var sampleDto = new SampleDto();

            //Action test = () => throw new ArgumentException();
            //test.Should().Throw<ArgumentException>();

            //sampleDto
            //    .Properties()
            //    .ThatArePublicOrInternal
            //    .ThatAreWritable
            //    .OfType<string>()
            //    .Should()
            //    .ProvideSymmetricAccess("test string");

            //sampleDto
            //    .Properties()
            //    .ThatArePublicOrInternal
            //    .ThatAreWritable
            //    .OfType<string>()
            //    .WhenCalledWith("test")
            //    .Should()
            //    .ProvideSymmetricAccess();

            sampleDto.Properties(p => p.MyStringProperty2, p => p.MyIntReadOnlyProperty)
                .OfType<string>()
                .WhenCalledWith("test1")
                .Should()
                .ProvideSymmetricAccess();

            sampleDto.Properties(p => p.MyStringProperty2)
                .WhenCalledWith("throw")
                .Should()
                .ThrowFromSetter<ArgumentException>()
                .WithMessage("test");

            //sampleDto.Properties<SampleDto, string>()
            //    .HavingValue("test").Should().BeWritable();

                //.WhenCalledWith("throw")
                //.Should()
                //.ThrowFromSetter<ArgumentException>()
                //.WithMessage("test");

            //sampleDto
            //    .Properties()
            //    .ThatAreWritable
            //    .OfType<string>()
            //    .Should().ProvideSymetricAccess("test");

            //sampleDto.Properties(x => x.MyIntReadOnlyProperty)
            //    .OfType<int>()
            //    .Should().BeReadOnly();
            //sampleDto.GetType().GetProperty(nameof(sampleDto.MyStringProperty))
            //    .Should().BeReadOnly();
        }
    }
}
