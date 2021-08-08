using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace FluentAssertions.Properties.Assertions
{
    public class PropertyException<TException>
        where TException : Exception
    {
        public PropertyException(TException exception, string propertyName)
        {
            Exception = exception;
            PropertyName = propertyName;
        }

        public TException Exception { get; }

        public string PropertyName { get; }
    }

    public class PropertyExceptionCollection<TException> : IEnumerable<PropertyException<TException>>
        where TException : Exception
    {
        List<PropertyException<TException>> _propertyExceptions = new List<PropertyException<TException>>();

        public void Add(TException exception, string propertyName)
        {
            _propertyExceptions.Add(new PropertyException<TException>(exception, propertyName));
        }

        public IEnumerator<PropertyException<TException>> GetEnumerator()
        {
            return _propertyExceptions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
