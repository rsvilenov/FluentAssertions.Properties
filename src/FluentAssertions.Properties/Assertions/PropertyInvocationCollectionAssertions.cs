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
    [DebuggerNonUserCode]
    public class PropertyInvocationCollectionAssertions<TDeclaringType, TProperty>
        : PropertyInvocationCollectionAssertions<PropertyInvocationCollectionAssertions<TDeclaringType, TProperty>, TDeclaringType, TProperty>
    {
        internal PropertyInvocationCollectionAssertions(PropertyInvocationCollection<TDeclaringType, TProperty> value)
            : base(value)
        {
        }
    }

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
        public PropertyInvocationCollection<TDeclaringType, TProperty> Subject { get; }

        public AndConstraint<TAssertions> ProvideSymmetricAccess(string because = "", params object[] becauseArgs)
        {
            return ExceptionStackTrace.StartFromCurrentFrame(() =>
                                 ProvideSymmetricAccessInternal(because, becauseArgs));
        }

        private AndConstraint<TAssertions> ProvideSymmetricAccessInternal(string because, params object[] becauseArgs)
        {
            using (AssertionScope assertion = new AssertionScope())
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
                        Execute
                            .Assertion
                            .ForCondition(AreGetSetOperationsSymetric(propertyInfo.Name, propertyInvocationInfo.ValueDelegate.Invoke()))
                            .BecauseOf(because, becauseArgs)
                            .FailWith("Expected the get and set operations of property {0} to be symetric{reason}, but were not.", propertyInfo.Name);
                    }

                }
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

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

                    try
                    {
                        CallPropertyAccessors(evalType, propertyInvocationInfo, propertyName);

                        Execute.Assertion
                            .BecauseOf(because, becauseArgs)
                            .FailWith("Expected property {0} of property {1} to throw {2}{reason}, but no exception was thrown.",
                                accessorTypeFailMessagePart,
                                propertyName,
                                typeof(TException));
                    }
                    catch (Exception ex)
                    {
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

                        propertyExceptions.Add((TException)ex,
                            propertyName,
                            evalType);
                    }
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

        private void CallPropertyAccessors(PropertyAccessorEvaluation evalType, PropertyInvocationInfo<TDeclaringType, TProperty> propertyInvocationInfo, string propertyName)
        {
            TProperty value = propertyInvocationInfo.ValueDelegate.Invoke();

            if (evalType == PropertyAccessorEvaluation.Setter)
            {
                _propertyInvoker.SetValue(propertyName, value);
            }
            else if (evalType == PropertyAccessorEvaluation.Getter)
            {
                _propertyInvoker.GetValue(propertyName);
            }
        }

        private bool AreGetSetOperationsSymetric(string propertyName, TProperty value)
        {
            bool isSymmetric = false;
            try
            {
                _propertyInvoker.SetValue(propertyName, value);
                TProperty got = _propertyInvoker.GetValue(propertyName);
                isSymmetric = value.Equals(got);
            }
            catch (Exception ex)
            {
                Execute.Assertion
                    .FailWith($"Did not expect any exceptions for property {propertyName}, but got {ex}.");
            }

            return isSymmetric;
        }
    }
}
