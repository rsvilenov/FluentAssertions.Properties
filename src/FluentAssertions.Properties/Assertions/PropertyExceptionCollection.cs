using System;
using System.Collections;
using System.Collections.Generic;

namespace FluentAssertions.Properties.Assertions
{
    public class PropertyException<TException>
        where TException : Exception
    {
        public PropertyException(TException exception,
            string propertyName,
            PropertyAccessorEvaluationType accessorEvaluationType)
        {
            Exception = exception;
            PropertyName = propertyName;
            AccessorEvaluationType = accessorEvaluationType;
        }

        public TException Exception { get; }
        public string PropertyName { get; }
        public PropertyAccessorEvaluationType AccessorEvaluationType { get; set; }

    }

    public class PropertyExceptionCollection<TException> : IEnumerable<PropertyException<TException>>
        where TException : Exception
    {
        List<PropertyException<TException>> _propertyExceptions = new List<PropertyException<TException>>();

        public void Add(TException exception, string propertyName, PropertyAccessorEvaluationType accessorEvaluationType)
        {
            _propertyExceptions.Add(new PropertyException<TException>(exception, propertyName, accessorEvaluationType));
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
