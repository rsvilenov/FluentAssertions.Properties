#if NET5_0_OR_GREATER
namespace FluentAssertions.Properties.Tests.PublicApiTests.TestObjects
{
    internal record TestRecord
    {
        public int MyMutableProperty { get; set; }
        public int MyInitOnlyProperty { get; init; }
        public int MyReadOnlyProperty { get; }
    }
}
#endif
