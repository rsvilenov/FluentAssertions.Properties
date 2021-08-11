using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentAssertions.Properties.Tests.TestableObjects
{
    public class SampleDto
    {
        public int MyIntReadOnlyProperty { get; }
        public int MyIntProperty { get; set; }
        public string MyStringProperty 
        {

            get
            {
                return _myStringProperty2;
            }
            set
            {
                if (value == "throw")
                    throw new ArgumentException("test3");
                _myStringProperty2 = value;
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
