using FluentAssertions.Execution;
using FluentAssertions.Properties.Data;
using FluentAssertions.Properties.Data.Enums;
using FluentAssertions.Properties.Extensions;
using FluentAssertions.Properties.Invocation;
using System;
using System.Diagnostics;
using System.Reflection;

namespace FluentAssertions.Properties.Assertions
{
    /// <summary>
    /// Contains a number of methods to assert that the selected properties behave in the expected way.
    /// </summary>
    [DebuggerNonUserCode]
    public class PropertyInvocationCollectionAssertions<TDeclaringType, TProperty>
        : PropertyInvocationCollectionAssertions<PropertyInvocationCollectionAssertions<TDeclaringType, TProperty>, TDeclaringType, TProperty>
    {
        internal PropertyInvocationCollectionAssertions(PropertyInvocationCollection<TDeclaringType, TProperty> value)
            : base(value)
        {
        }
    }

    /// <summary>
    /// Contains a number of methods to assert that the selected properties behave in the expected way.
    /// </summary>
    [DebuggerNonUserCode]
    public class PropertyInvocationCollectionAssertions<TAssertions, TDeclaringType, TProperty>
        where TAssertions : PropertyInvocationCollectionAssertions<TAssertions, TDeclaringType, TProperty>
    {
        private readonly IPropertyInvoker<TProperty> _propertyInvoker;

        internal PropertyInvocationCollectionAssertions(PropertyInvocationCollection<TDeclaringType, TProperty> value)
        {
            Subject = value;
            _propertyInvoker = InvocationContext.PropertyInvokerFactory.CreatePropertyInvoker<TDeclaringType, TProperty>(value.Instance);
        }

        /// <summary>
        /// Gets the object whose value is being asserted.
        /// </summary>
        internal PropertyInvocationCollection<TDeclaringType, TProperty> Subject { get; }

        /// <summary>
        /// Asserts that the selected properties return the same value as the one that has been assigned to them.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> ProvideSymmetricAccess(string because = "", params object[] becauseArgs)
        {
            return ExceptionStackTrace.StartFromCurrentFrame(() =>
                                 ProvideSymmetricAccessInternal(because, becauseArgs));
        }

        private AndConstraint<TAssertions> ProvideSymmetricAccessInternal(string because, params object[] becauseArgs)
        {
            using (AssertionScope scope = new AssertionScope())
            {
                foreach (var propertyInvocationInfo in Subject)
                {
                    PropertyInfo propertyInfo = propertyInvocationInfo
                        .PropertyInfo;
                    
                    if (!propertyInfo.CanWrite)
                    {
                        Execute
                            .Assertion
                            .BecauseOf(because, becauseArgs)
                            .FailWith("Expected property {0} to be writable{reason}, but was not.", propertyInfo.Name);
                    }
                    else
                    {
                        IInvocationResult<TProperty> valueSourceInvocationResult = propertyInvocationInfo
                            .ValueSourceInvocationDelegate
                            .Invoke();
                        if (!valueSourceInvocationResult.Success)
                        {
                            var valueSourceInvocationException = valueSourceInvocationResult.ExceptionDispatchInfo.SourceException;
                            Execute.Assertion
                                    .BecauseOf(because, becauseArgs)
                                    .FailWith("Did not expect any exceptions when getting the value to be passed to property {0}, but {1} was thrown.{2} {3}",
                                        propertyInfo.Name,
                                        valueSourceInvocationException.GetType(),
                                        Environment.NewLine,
                                        valueSourceInvocationException);
                            continue;
                        }
                        
                        Execute
                            .Assertion
                            .ForCondition(AreGetSetOperationsSymmetric(propertyInfo.Name, valueSourceInvocationResult.Value))
                            .BecauseOf(because, becauseArgs)
                            .FailWith("Expected the get and set operations of property {0} to be symmetric{reason}, but were not.", propertyInfo.Name);
                    }

                }
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the selected properties throw from their setters an exception of type <typeparamref name="TException" />.
        /// </summary>
        /// <typeparam name="TException">The expected type of exception.</typeparam>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public PropertyExceptionCollectionAssertions<TException> ThrowFromSetter<TException>(string because = "", params object[] becauseArgs)
            where TException : Exception
        {
            return ExceptionStackTrace.StartFromCurrentFrame(() =>
                Throw<TException>(
                    PropertyAccessorEvaluation.Setter,
                    matchExactExceptionType: false,
                    because,
                    becauseArgs));
        }

        /// <summary>
        /// Asserts that the selected properties throw from their getters an exception of type <typeparamref name="TException" />.
        /// </summary>
        /// <typeparam name="TException">The expected type of exception.</typeparam>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public PropertyExceptionCollectionAssertions<TException> ThrowFromGetter<TException>(string because = "", params object[] becauseArgs)
            where TException : Exception
        {
            return ExceptionStackTrace.StartFromCurrentFrame(() =>
                Throw<TException>(
                    PropertyAccessorEvaluation.Getter,
                    matchExactExceptionType: false, 
                    because, 
                    becauseArgs));
        }

        /// <summary>
        /// Asserts that the selected properties throw from their setters an exception of type <typeparamref name="TException" /> (and not a derived exception type).
        /// </summary>
        /// <typeparam name="TException">The expected type of exception.</typeparam>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public PropertyExceptionCollectionAssertions<TException> ThrowFromSetterExactly<TException>(string because = "", params object[] becauseArgs)
            where TException : Exception
        {
            return ExceptionStackTrace.StartFromCurrentFrame(() =>
                Throw<TException>(
                    PropertyAccessorEvaluation.Setter,
                    matchExactExceptionType: true,
                    because,
                    becauseArgs));
        }

        /// <summary>
        /// Asserts that the selected properties throw from their getters an exception of type <typeparamref name="TException" /> (and not a derived exception type).
        /// </summary>
        /// <typeparam name="TException">The expected type of exception.</typeparam>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public PropertyExceptionCollectionAssertions<TException> ThrowFromGetterExactly<TException>(string because = "", params object[] becauseArgs)
            where TException : Exception
        {
            return ExceptionStackTrace.StartFromCurrentFrame(() =>
                Throw<TException>(
                    PropertyAccessorEvaluation.Getter,
                    matchExactExceptionType: true,
                    because,
                    becauseArgs));
        }

        private PropertyExceptionCollectionAssertions<TException> Throw<TException>(PropertyAccessorEvaluation evalType, bool matchExactExceptionType, string because = "", params object[] becauseArgs)
            where TException : Exception
        {
            PropertyExceptionCollection<TException> propertyExceptions = new PropertyExceptionCollection<TException>();

            string accessorTypeFailMessagePart = evalType.GetDescription();

            using (AssertionScope assertion = new AssertionScope())
            {
                foreach (var propertyInvocationInfo in Subject)
                {
                    string propertyName = propertyInvocationInfo
                        .PropertyInfo
                        .Name;

                    IInvocationResult<TProperty> valueSourceInvocationResult = propertyInvocationInfo
                        .ValueSourceInvocationDelegate
                        .Invoke();
                    if (!valueSourceInvocationResult.Success)
                    {
                        var valueSourceInvocationException = valueSourceInvocationResult.ExceptionDispatchInfo.SourceException;
                        Execute.Assertion
                                    .BecauseOf(because, becauseArgs)
                                    .FailWith("Did not expect any exceptions when getting the value to be passed to property {0}, but {1} was thrown. {2}",
                                        propertyName,
                                        valueSourceInvocationException.GetType(),
                                        valueSourceInvocationException);
                        continue;
                    }

                    TProperty sourceValue = valueSourceInvocationResult.Value;
                    IInvocationResult invocationResult = evalType == PropertyAccessorEvaluation.Setter
                        ? _propertyInvoker.SetValue(propertyName, sourceValue)
                        : _propertyInvoker.GetValue(propertyName);

                    if (!invocationResult.Success)
                    {
                        Exception ex = invocationResult.ExceptionDispatchInfo.SourceException;
                        bool exceptionTypeMatches = MatchExceptionType<TException>(matchExactExceptionType, ex);

                        if (!exceptionTypeMatches)
                        {
                            Execute.Assertion
                                .BecauseOf(because, becauseArgs)
                                .FailWith("Expected property {0} of property {1} to throw {2}{reason}, but {3} was thrown. {4}",
                                    accessorTypeFailMessagePart,
                                    propertyName,
                                    typeof(TException),
                                    ex.GetType(),
                                    ex);
                        }
                        else
                        {
                            propertyExceptions.Add((TException)ex,
                                propertyName,
                                evalType);
                        }

                        continue;
                    }

                    Execute.Assertion
                            .BecauseOf(because, becauseArgs)
                            .FailWith("Expected property {0} of property {1} to throw {2}{reason}, but no exception was thrown.",
                                accessorTypeFailMessagePart,
                                propertyName,
                                typeof(TException));
                }
            }

            return new PropertyExceptionCollectionAssertions<TException>(propertyExceptions);
        }

        private bool MatchExceptionType<TException>(bool matchExactExceptionType, Exception ex) where TException : Exception
        {
            return matchExactExceptionType
                ? ex.GetType().Equals(typeof(TException))
                : ex is TException;
        }

        private bool AreGetSetOperationsSymmetric(string propertyName, TProperty value)
        {
            bool isSymmetric = false;
            IInvocationResult setResult = _propertyInvoker.SetValue(propertyName, value);

            if (!setResult.Success)
            {
                Execute.Assertion
                    .FailWith($"Did not expect any exceptions for property {propertyName}, but the setter threw {setResult.ExceptionDispatchInfo.SourceException}.");
            }
            else
            {
                IInvocationResult<TProperty> getResult = _propertyInvoker.GetValue(propertyName);
                if (!getResult.Success)
                {
                    Execute.Assertion
                        .FailWith($"Did not expect any exceptions for property {propertyName}, but the getter threw {getResult.ExceptionDispatchInfo.SourceException}.");
                }

                isSymmetric = Equals(getResult.Value, value);
            }

            return isSymmetric;
        }
    }
}
