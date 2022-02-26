using FluentAssertions.Properties.Extensions;
using Xunit;

namespace FluentAssertions.Properties.Tests.Extensions
{
    public class ExtensionMethodTests
    {
        public class PrivatePropertyHolder
        {
            private string TestProperty { get; }

            // write-only properties are not supported by the library
            private string SetOnlyProperty { set { } }

            [Fact]
            public void When_GetPublicOrInternalProperties_is_called_on_private_or_setter_only_properties_it_should_not_return_results()
            {
                typeof(PrivatePropertyHolder)
                    .GetPublicOrInternalProperties()
                    .Should()
                    .BeEmpty();
            }
        }

    }
}
