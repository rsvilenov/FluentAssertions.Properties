namespace FluentAssertions.Properties.Tests.PublicApiTests.TestObjects
{
    public class TestSubClass : TestClassBase
    {
        public int IntProperty { get; set; }
        public double DoubleProperty { get; set; }
        public string StringProperty { get; set; }
        internal string InternalStringProperty { get; set; }
        public string ReadOnlyStringProperty { get; }
    }
}
