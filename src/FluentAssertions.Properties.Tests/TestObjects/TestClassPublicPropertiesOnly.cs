﻿namespace FluentAssertions.Properties.Tests.TestObjects
{
    public class TestClassPublicPropertiesOnly
    {
        public int IntProperty { get; set; }
        public double DoubleProperty { get; set; }
        public string StringProperty { get; set; }
        public string ReadOnlyStringProperty { get; }
    }
}