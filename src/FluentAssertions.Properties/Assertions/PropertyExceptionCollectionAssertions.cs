using FluentAssertions.Execution;
using FluentAssertions.Properties.Data;
using FluentAssertions.Properties.Extensions;
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

        internal PropertyExceptionCollectionAssertions(PropertyExceptionCollection<TException> exceptionCollection,
            List<string> innerExceptionStack = null)
        {
            _exceptionCollection = exceptionCollection;
            _innerExceptionStack = innerExceptionStack ?? new List<string>();
        }

        public PropertyExceptionCollectionAssertions<TException> And => this;

        public virtual PropertyExceptionCollectionAssertions<TException> WithMessage(string expectedWildcardPattern, string because = "",
                    params object[] becauseArgs)
        {
            return ExceptionStackTrace.StartFromCurrentFrame(() =>
                WithMessageInternal(expectedWildcardPattern, because, becauseArgs));
        }

        private PropertyExceptionCollectionAssertions<TException> WithMessageInternal(string expectedWildcardPattern, string because, object[] becauseArgs)
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
            return ExceptionStackTrace.StartFromCurrentFrame(() =>
                WhereInternal(exceptionExpression, because, becauseArgs));
        }

        private PropertyExceptionCollectionAssertions<TException> WhereInternal(Expression<Func<TException, bool>> exceptionExpression, string because, object[] becauseArgs)
        {
            Func<TException, bool> condition = exceptionExpression.Compile();

            using (AssertionScope scope = new AssertionScope())
            {
                foreach (PropertyException<TException> pex in _exceptionCollection)
                {
                    using (var innerScope = Execute.Assertion)
                    {
                        innerScope.Context = pex.AccessorEvaluationType.GetDescription();
                        innerScope.ForCondition(condition(pex.Exception))
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected an exception {0} for the {context} of property {1} where {2}{reason}, but the condition was not met by:{3}{3}{4}.",
                            GetInnerExceptionStack<TException>(),
                            pex.PropertyName,
                            exceptionExpression.Body,
                            Environment.NewLine,
                            pex.Exception);
                    }
                }
            }

            return this;
        }

        public PropertyExceptionCollectionAssertions<TInnerException> WithInnerException<TInnerException>(string because = null,
            params object[] becauseArgs)
            where TInnerException : Exception
        {
            return ExceptionStackTrace.StartFromCurrentFrame(() =>
                WithInnerExceptionInternal<TInnerException>(matchExactType: false, because, becauseArgs));
        }
    
        public PropertyExceptionCollectionAssertions<TInnerException> WithInnerExceptionExactly<TInnerException>(string because = null,
            params object[] becauseArgs)
            where TInnerException : Exception
        {
            return ExceptionStackTrace.StartFromCurrentFrame(() =>
                   WithInnerExceptionInternal<TInnerException>(matchExactType: true, because, becauseArgs));
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
                    if (pex.Exception.InnerException == null)
                    {
                        // format the message beforehand, so that FailWith() will not enclose the placeholders with brackets
                        string failMessage = string.Format("the {0} exception has no inner exception.",
                                _innerExceptionStack.Any() ? "inner" : "thrown");

                        using (var innerScope = Execute.Assertion)
                        {
                            innerScope.Context = pex.AccessorEvaluationType.GetDescription();
                            innerScope.BecauseOf(because, becauseArgs)
                            .WithExpectation("Expected inner {0}{reason} for the {context} of property {1}, but ",
                                GetInnerExceptionStack<TInnerException>(),
                                pex.PropertyName)
                            .FailWith(failMessage);
                        }
                    }
                    else
                    {
                        using (var innerScope = Execute.Assertion)
                        {
                            innerScope.Context = pex.AccessorEvaluationType.GetDescription();
                            innerScope.ForCondition(matchExactType
                                ? pex.Exception.InnerException.GetType().Equals(typeof(TInnerException))
                                : pex.Exception.InnerException is TInnerException)
                            .BecauseOf(because, becauseArgs)
                            .WithExpectation("Expected inner {0}{reason} for the {context} of property {1}, but ",
                                GetInnerExceptionStack<TInnerException>(),
                                pex.PropertyName)
                            .FailWith("found {1}.",
                                typeof(TInnerException), pex.Exception.InnerException);
                        }
                    }
                }
            }

            foreach (PropertyException<TException> pex in _exceptionCollection)
            {
                innerExceptionCollection.Add((TInnerException)pex.Exception.InnerException,
                    pex.PropertyName,
                    pex.AccessorEvaluationType);
            }

            _innerExceptionStack.Add(typeof(TInnerException).Name);

            return new PropertyExceptionCollectionAssertions<TInnerException>(innerExceptionCollection, _innerExceptionStack);
        }

        // TODO: The TException will always be System.Exception for lists where the generic parameter is the broad <Exception>.
        private string GetInnerExceptionStack<TInnerException>() 
            where TInnerException : Exception
        {
            const string delimiter = "->";
            StringBuilder sb = new StringBuilder();

            sb.Append(typeof(TException).Name);
            sb.Append(delimiter);
            if (_innerExceptionStack.Any())
            {
                sb.Append(string.Join(delimiter, _innerExceptionStack));
                sb.Append(delimiter);
            }

            sb.Append(typeof(TInnerException).Name);

            return sb.ToString();
        }
    }
}
