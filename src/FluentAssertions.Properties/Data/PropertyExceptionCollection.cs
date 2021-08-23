using FluentAssertions.Properties.Data.Enums;
using System;
using System.Collections;
using System.Collections.Generic;

namespace FluentAssertions.Properties.Data
{
    internal class PropertyException<TException>
        where TException : Exception
    {
        internal PropertyException(TException exception,
            string propertyName,
            PropertyAccessorEvaluation accessorEvaluationType)
        {
            Exception = exception;
            PropertyName = propertyName;
            AccessorEvaluationType = accessorEvaluationType;
        }

        public TException Exception { get; }
        public string PropertyName { get; }
        public PropertyAccessorEvaluation AccessorEvaluationType { get; }

    }

    internal class PropertyExceptionCollection<TException> : IEnumerable<PropertyException<TException>>
        where TException : Exception
    {
        private readonly List<PropertyException<TException>> _propertyExceptions = new List<PropertyException<TException>>();

        internal void Add(TException exception, string propertyName, PropertyAccessorEvaluation accessorEvaluationType)
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
