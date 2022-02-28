using FluentAssertions.Properties.Extensions;
using System.ComponentModel;
using Xunit;

namespace FluentAssertions.Properties.Tests.InternalTests
{
    public class EnumExtensionsTests
    {
        const string EnumMemberDescription = "test";

        enum DecoratedEnum
        {
            [Description(EnumMemberDescription)]
            MemberWithDescription,
            MemberWithoutDescription
        }

        [Fact]
        public void When_GetDescription_is_called_on_decorated_enum_member_it_should_return_the_description()
        {
            // Arrange
            var decoratedMember = DecoratedEnum.MemberWithDescription;

            // Act
            string description = decoratedMember.GetDescription();

            // Assert
            description.Should().Be(EnumMemberDescription);
        }

        [Fact]
        public void When_GetDescription_is_called_on_undecorated_enum_member_it_should_return_the_member_name()
        {
            // Arrange
            var undecoratedMember = DecoratedEnum.MemberWithoutDescription;
            string undecoratedMemberName = undecoratedMember.ToString();

            // Act
            string description = undecoratedMember.GetDescription();

            // Assert
            description.Should().Be(undecoratedMemberName);
        }
    }
}
