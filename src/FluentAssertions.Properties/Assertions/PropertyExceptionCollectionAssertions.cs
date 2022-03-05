using FluentAssertions.Execution;
using FluentAssertions.Properties.Common;
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
    /// <summary>
    /// Contains a number of methods to assert that the thrown exceptions from the selected properties are in the correct state.
    /// </summary>
    [DebuggerNonUserCode]
    public class PropertyExceptionCollectionAssertions<TException>
        where TException : Exception
    {
        private readonly List<string> _innerExceptionPedigree;
        private readonly PropertyExceptionCollection<TException> _exceptionCollection;

        internal PropertyExceptionCollectionAssertions(PropertyExceptionCollection<TException> exceptionCollection)
            : this(exceptionCollection, new List<string>())
        {
        }

        private PropertyExceptionCollectionAssertions(PropertyExceptionCollection<TException> exceptionCollection,
            IEnumerable<string> innerExceptionPedigree)
        {
            _exceptionCollection = exceptionCollection;
            _innerExceptionPedigree = innerExceptionPedigree.ToList();
            _innerExceptionPedigree.Add(typeof(TException).Name);
        }

        /// <summary>
        /// Asserts that the exceptions thrown from the selected properties have a message that matches <paramref name="expectedWildcardPattern" />.
        /// </summary>
        /// <param name="expectedWildcardPattern">
        /// The pattern to match against the exception message.
        /// For details on the format of this pattern, please consult
        /// the similar method <see cref="FluentAssertions.Specialized.ExceptionAssertions.WithMessage(string, string, object[])"/>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
        /// </param>
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

        /// <summary>
        /// Asserts that the exceptions thrown from the selected properties match a particular condition.
        /// </summary>
        /// <param name="exceptionExpression">
        /// The condition that the exceptions must match.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="exceptionExpression"/> is <c>null</c>.</exception>
        public virtual PropertyExceptionCollectionAssertions<TException> Where(Expression<Func<TException, bool>> exceptionExpression,
            string because = "",
            params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(exceptionExpression, nameof(exceptionExpression));

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
                            ConstructExceptionPedigree(),
                            pex.PropertyName,
                            exceptionExpression.Body,
                            Environment.NewLine,
                            pex.Exception);
                    }
                }
            }

            return this;
        }

        /// <summary>
        /// Asserts that the exceptions thrown from the selected properties
        /// contains an inner exception of type <typeparamref name="TInnerException" />.
        /// </summary>
        /// <typeparam name="TInnerException">The expected type of the inner exception.</typeparam>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public PropertyExceptionCollectionAssertions<TInnerException> WithInnerException<TInnerException>(string because = null,
            params object[] becauseArgs)
            where TInnerException : Exception
        {
            return ExceptionStackTrace.StartFromCurrentFrame(() =>
                WithInnerExceptionInternal<TInnerException>(matchExactType: false, because, becauseArgs));
        }

        /// <summary>
        /// Asserts that the exceptions thrown from the selected properties
        /// contains an inner exception of the exact type <typeparamref name="TInnerException" /> 
        /// (and not a derived exception type).
        /// </summary>
        /// <typeparam name="TInnerException">The expected type of the inner exception.</typeparam>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
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
                                _innerExceptionPedigree.Count > 1 ? "inner" : "thrown");

                        using (var innerScope = Execute.Assertion)
                        {
                            innerScope.Context = pex.AccessorEvaluationType.GetDescription();
                            innerScope.BecauseOf(because, becauseArgs)
                            .WithExpectation("Expected inner {0}{reason} for the {context} of property {1}, but ",
                                ConstructExceptionPedigree(typeof(TInnerException)),
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
                                ConstructExceptionPedigree(typeof(TInnerException)),
                                pex.PropertyName)
                            .FailWith("found {0}.",
                                ConstructExceptionPedigree(pex.Exception.InnerException.GetType()));
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

            return new PropertyExceptionCollectionAssertions<TInnerException>(innerExceptionCollection, _innerExceptionPedigree);
        }

        private string ConstructExceptionPedigree(Type lastInnerExType = null)
        {
            const string delimiter = "->";
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Join(delimiter, _innerExceptionPedigree));

            if (lastInnerExType != null)
            {
                sb.Append(delimiter);
                sb.Append(lastInnerExType.Name);
            }

            return sb.ToString();
        }
    }
}
