using AutoFixture;
using FluentAssertions.Properties.Tests.TestableObjects;
using System;
using System.ComponentModel;
using System.Linq;
using Xunit;

namespace FluentAssertions.Properties.Assertions
{
    public class PropertyAssertionsTests
    {
        struct TestStruct
        {
            public string MyProperty { get; set; }
            public TestStruct(string str)
            {
                MyProperty = str;
            }
        }
        [Fact]
        public void Test1()
        {
            // Arrange
            var sampleDto = new SampleDto();

            sampleDto.Properties()
                .OfType<int>()
                .ThatAreWritable
                .WhenCalledWithValuesFrom(new Fixture().Create<SampleDto>())
                .Should()
                .ProvideSymmetricAccess();

            sampleDto.Properties().Should().BeOfPrimitiveType();
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

            //sampleDto.Properties(p => p.MyStringProperty2, p => p.MyIntReadOnlyProperty)
            //    .OfType<string>()
            //    .WhenCalledWith("test1")
            //    .Should()
            //    .NotThrowFromSetter<NullReferenceException>()
            //    .And
            //    .Should()
            //    .ProvideSymmetricAccess();

            sampleDto.Properties(p => p.MyStringProperty)
                .WhenCalledWith("throw")
                .Should()
                .ThrowFromSetterExactly<ArgumentException>()
                .WithInnerException<NullReferenceException>()
                .WithInnerException<Win32Exception>();
                //.Where(ex => ex.Message.StartsWith("ate"), "some reason");
                //.WithMessage("test2", "some reason");



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
