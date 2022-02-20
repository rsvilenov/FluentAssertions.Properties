namespace FluentAssertions.Properties.Tests.PublicApi.TestObjects
{
    public class TestClass : TestClassBase
    {
        public int IntProperty { get; set; }
        public double DoubleProperty { get; set; }
        public string StringProperty { get; set; }
        public string StringPropertyWithInternalSetter { get; internal set; }
        public string StringPropertyWithPrivateSetter { get; private set; }
        private string PrivateStringProperty { get; set; }
        internal string InternalStringProperty { get; set; }
        public string ReadOnlyStringProperty { get; }

        public int? NullableValueTypeProperty { get; set; }
        public EmptyTestClass UserTypeProperty { get; set; }

        public virtual bool VirtualProperty { get; set; }
    }
}
