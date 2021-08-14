using FluentAssertions.Execution;
using FluentAssertions.Specialized;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace FluentAssertions.Properties.Assertions
{
    [DebuggerNonUserCode]
    public class PropertyExceptionCollectionAssertions<TException>
        where TException : Exception
    {
        private readonly List<string> _innerExceptionStack;
        private readonly PropertyExceptionCollection<TException> _exceptionCollection;
        public PropertyExceptionCollectionAssertions(PropertyExceptionCollection<TException> exceptionCollection,
            List<string> innerExceptionStack = null)
        {
            _exceptionCollection = exceptionCollection;
            _innerExceptionStack = innerExceptionStack ?? new List<string>();
        }

        public PropertyExceptionCollectionAssertions<TException> And => this;

        public virtual PropertyExceptionCollectionAssertions<TException> WithMessage(string expectedWildcardPattern, string because = "",
                    params object[] becauseArgs)
        {
            using (AssertionScope scope = new AssertionScope())
            {
                foreach (PropertyException<TException> pex in _exceptionCollection)
                {
                    scope.Context = $"the {pex.Exception.GetType().Name} message for property {pex.PropertyName}";
                    pex.Exception.Message.Should().MatchEquivalentOf(expectedWildcardPattern, because, becauseArgs);
                }
            }

            return this;
        }

        public virtual PropertyExceptionCollectionAssertions<TException> Where(Expression<Func<TException, bool>> exceptionExpression,
            string because = "",
            params object[] becauseArgs)
        {
            Func<TException, bool> condition = exceptionExpression.Compile();

            using (AssertionScope scope = new AssertionScope())
            {
                foreach (PropertyException<TException> pex in _exceptionCollection)
                {
                    Execute.Assertion
                        .ForCondition(condition(pex.Exception))
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected exception{0} for property {1} where {2}{reason}, but the condition was not met by:{3}{3}{4}.",
                            GetInnerExceptionStack<TException>(),
                            pex.PropertyName, 
                            exceptionExpression.Body, 
                            Environment.NewLine, 
                            pex.Exception);
                }
            }

            return this;
        }

        public PropertyExceptionCollectionAssertions<TInnerException> WithInnerException<TInnerException>(string because = null,
            params object[] becauseArgs)
            where TInnerException : Exception
        {
            return WithInnerExceptionInternal<TInnerException>(matchExactType: false, because, becauseArgs);
        }

        public PropertyExceptionCollectionAssertions<TInnerException> WithInnerExceptionExactly<TInnerException>(string because = null,
            params object[] becauseArgs)
            where TInnerException : Exception
        {
            return WithInnerExceptionInternal<TInnerException>(matchExactType: true, because, becauseArgs);
        }

        private PropertyExceptionCollectionAssertions<TInnerException> WithInnerExceptionInternal<TInnerException>(bool matchExactType,
            string because = null,
            params object[] becauseArgs)
            where TInnerException : Exception
        {
            PropertyExceptionCollection<TInnerException> innerExceptionCollection = new PropertyExceptionCollection<TInnerException>();
            using (AssertionScope scope = new AssertionScope())
            {
                foreach (PropertyException<TException> pex in _exceptionCollection)
                {
                    Execute.Assertion
                        .BecauseOf(because, becauseArgs)
                        .WithExpectation("Expected inner {0}{reason}, but ",
                            GetInnerExceptionStack<TInnerException>())
                        .ForCondition(pex.Exception.InnerException != null)
                        .FailWith("the {0} exception has no inner exception.",
                            _innerExceptionStack.Any() ? "inner" : "thrown")
                        .Then
                        .ClearExpectation();

                    Execute.Assertion
                        .ForCondition(matchExactType 
                            ? pex.Exception.InnerException.GetType().Equals(typeof(TInnerException)) 
                            : pex.Exception.InnerException is TInnerException)
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected inner {0}{reason}, but found {1}.",
                            typeof(TInnerException), pex.Exception.InnerException);

                    innerExceptionCollection.Add((TInnerException)pex.Exception.InnerException, pex.PropertyName);
                }
            }

            _innerExceptionStack.Add(typeof(TInnerException).Name);

            return new PropertyExceptionCollectionAssertions<TInnerException>(innerExceptionCollection, _innerExceptionStack);
        }

        private string GetInnerExceptionStack<TInnerException>() 
            where TInnerException : Exception
        {
            StringBuilder sb = new StringBuilder();

            if (_innerExceptionStack.Any())
            {
                sb.Append(string.Join("->", _innerExceptionStack));
            }

            sb.Append(typeof(TInnerException).Name);

            return sb.ToString();
        }
    }
}
