namespace FluentAssertions.Properties.Tests.PublicApiTests.TestObjects
{
    public interface ITestProperties
    {
        string StringProperty { get; set; }
        int IntProperty { get; set; }
        string ReadOnlyStringProperty { get; }
    }
}
