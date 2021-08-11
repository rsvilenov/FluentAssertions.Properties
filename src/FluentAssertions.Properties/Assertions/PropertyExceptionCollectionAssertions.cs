using FluentAssertions.Execution;
using FluentAssertions.Specialized;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentAssertions.Properties.Assertions
{
    public class PropertyExceptionCollectionAssertions<TException>
        where TException : Exception
    {
        private readonly PropertyExceptionCollection<TException> _exceptionCollection;
        public PropertyExceptionCollectionAssertions(PropertyExceptionCollection<TException> exceptionCollection)
        {
            _exceptionCollection = exceptionCollection;
        }

        public virtual PropertyExceptionCollectionAssertions<TException> WithMessage(string expectedWildcardPattern, string because = "",
                    params object[] becauseArgs)
        {
            using (AssertionScope assertion = Execute.Assertion.BecauseOf(because, becauseArgs).UsingLineBreaks)
            {
                using (AssertionScope innerScope = new AssertionScope("context1"))
                {
                    foreach (PropertyException<TException> pex in _exceptionCollection)
                    {
                        innerScope.Context = $"the {pex.Exception.GetType().Name} message for property {pex.PropertyName}";
                        pex.Exception.Message.Should().MatchEquivalentOf(expectedWildcardPattern, because, becauseArgs);
                    }
                }
            }

            return this;
        }
    }
}
