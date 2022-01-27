namespace FluentAssertions.Properties.Tests.TestObjects
{
    public interface ITestProperties
    {
        string StringProperty { get; set; }
        int IntProperty { get; set; }
        string ReadOnlyStringProperty { get; }
    }
}
