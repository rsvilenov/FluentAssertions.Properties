namespace FluentAssertions.Properties.Tests.PublicApi.TestObjects
{
    public class TestClassValueTypePropertiesOnly
    {
        public const string NonDefaultValueSuffix = "WithNonDefaultValue";
        public int IntProperty { get; set; }
        public int? NullableIntProperty { get; set; }
        public int IntPropertyWithNonDefaultValue { get; set; } = 1;
        public double DoubleProperty { get; set; }
        public double? NullableDoubleProperty { get; set; }
        public double DoublePropertyWithNonDefaultValue { get; set; } = 1d;
        public bool BoolProperty { get; set; }
        public bool? NullableBoolProperty { get; set; }
        public bool BoolPropertyWithNonDefaultValue { get; set; } = true;
    }
}
