using System;
using System.Diagnostics.CodeAnalysis;

namespace FluentAssertions.Properties.Tests.TestObjects
{
    public class SampleDto
    {
        public int MyIntReadOnlyProperty { get; }
        public int MyIntProperty { get; set; }

        private string _myStringProperty;
        public string MyStringProperty 
        {

            get
            {
                return _myStringProperty;
            }
            set
            {
                if (value == "throw")
                    throw new ArgumentException("test3", new NullReferenceException("haha", new ArgumentOutOfRangeException()));
                _myStringProperty = value;
            }
        }

        private string _myStringProperty2;
        public string MyStringProperty2
        {
            get
            {
                return _myStringProperty2;
            }
            set
            {
                if (value == "throw")
                    throw new ArgumentException("test");
                _myStringProperty2 = value;
            }
        }
    }
}
